using Survey.Services;
using System.Data;
using System.Diagnostics;

namespace Survey.Pages;

public partial class SurveyPage : ContentPage
{
    public static Color SelectedStroke = Color.FromArgb("#B8573C");
    public static Color DefaultStroke = Color.FromArgb("#E7E0D7");
    private static readonly Color FieldStroke = Color.FromArgb("#B8573C");
    private static readonly Color ErrorStroke = Color.FromArgb("#D32F2F");

    private SurveyState _state;
    private readonly Dictionary<Border, (RadioButton Radio, string Ownership, string Hint)> _orderOptions;
    private readonly Dictionary<CheckBox, string> _checkOptions;

    private bool _showErrors;   // подсвечивать ошибки только после первой попытки перейти дальше
    private bool _vetTouched;   // переключатель ветеринарии считается заполненным, когда его трогали

    public SurveyPage(SurveyState state)
    {
        InitializeComponent();
        _state = state;
        _orderOptions = new()
        {
            [YesBorder] = (YesOption, "у вас есть кот или два)", "Сколько у вас котиков?"),
            [PlaningBorder] = (PlaningOption, "вы планируете завести котика)", "Сколько планируете котиков?"),
            [LoveBorder] = (LoveOption, "вы просто любите котиков)", "Сколько бы вы хотели котиков?"),
        };
        _checkOptions = new()
        {
            [PlayfulBox] = "игривый исследователь",
            [FreeBox] = "несгибаемый дух",
            [CuteBox] = "милый спутник",
            [ShyBox] = "робкая душа",
        };

        UpdateOptionalFields();
    }

    private void OnBreedChanged(object sender, EventArgs e)
    {
        _state.CatBreed = (sender as Picker)?.SelectedItem as string;
        Refresh();
    }

    private void OnCatNameChanged(object sender, TextChangedEventArgs e)
    {
        _state.CatName = e.NewTextValue;
        Refresh();
    }

    private void OnCatAgeChanged(object sender, TextChangedEventArgs e)
    {
        _state.CatMass = int.TryParse(e.NewTextValue, out var mass) ? mass : 0;
        Refresh();
    }

    private void OnTappedCatOption(object sender, TappedEventArgs e)
    {
        if (sender is Border border && _orderOptions.TryGetValue(border, out var option))
            option.Radio.IsChecked = true;
    }

    private void OnCatOptionChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value || sender is not RadioButton radio) return;

        var (border, option) = _orderOptions.First(pair => pair.Value.Radio == radio);

        foreach (var key in _orderOptions.Keys)
            key.Stroke = key == border ? SelectedStroke : DefaultStroke;

        _state.CatOwnership = option.Ownership;
        HintLabel.Text = option.Hint;

        UpdateOptionalFields();
        Refresh();
    }

    private void OnStepperValueChanged(object sender, EventArgs e)
    {
        _state.CatsCount = (int)((Stepper)sender).Value;
    }

    private async void GoToMain(object sender, EventArgs e)
    {
        _state.Reset();
        PagesNavigation.GoToAsync(PagesNames.MainPage);
    }

    private async void GoToResults(object sender, EventArgs e)
    {
        _showErrors = true;
        var firstInvalid = ValidateForm();

        if (firstInvalid is not null)
        {
            await FormScroll.ScrollToAsync(firstInvalid, ScrollToPosition.Center, true);
            return;
        }

        PagesNavigation.GoToAsync(PagesNames.ResultsPage);
    }

    private void OnTappedChecked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is CheckBox option)
        {
            option.IsChecked = !option.IsChecked;
        }
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox check) return;
        var trait = _checkOptions[check];

        if (e.Value)
            _state.CatTraits.Add(trait);
        else
            _state.CatTraits.Remove(trait);
    }

    private void OnVetToggled(object sender, ToggledEventArgs e)
    {
        _vetTouched = true;
        _state.CatHealthy = e.Value;
        Refresh();
    }

    private void OnReceivedDateSelected(object sender, DateChangedEventArgs e)
    {
        _state.CatDate = e.NewDate ?? DateTime.Today;
    }

    private void OnNotesChanged(object sender, TextChangedEventArgs e)
    {
        _state.CatInfo = e.NewTextValue;
    }

    // Если кот в планах, вес и ветеринария необязательны: звёздочки скрываются
    private void UpdateOptionalFields()
    {
        bool isPlanned = PlaningOption.IsChecked || LoveOption.IsChecked;

        WeightStar.IsVisible = !isPlanned;
        VetStar.IsVisible = !isPlanned;
    }

    private void Refresh()
    {
        if (_showErrors)
            ValidateForm();
    }

    // Возвращает первое незаполненное поле или null, если форма заполнена
    private VisualElement? ValidateForm()
    {
        bool isPlanned = PlaningOption.IsChecked || LoveOption.IsChecked;

        bool ownershipOk = _orderOptions.Values.Any(o => o.Radio.IsChecked);
        bool nameOk = !string.IsNullOrWhiteSpace(NameEntry.Text);
        bool breedOk = CatPicker.SelectedIndex >= 0;
        bool weightOk = isPlanned || !string.IsNullOrWhiteSpace(WeightEntry.Text);
        bool vetOk = isPlanned || _vetTouched;

        foreach (var (border, option) in _orderOptions)
        {
            border.Stroke = !ownershipOk ? ErrorStroke
                          : option.Radio.IsChecked ? SelectedStroke
                          : DefaultStroke;
        }

        NameBorder.Stroke = nameOk ? FieldStroke : ErrorStroke;
        BreedBorder.Stroke = breedOk ? FieldStroke : ErrorStroke;
        WeightBorder.Stroke = weightOk ? FieldStroke : ErrorStroke;
        VetBorder.Stroke = vetOk ? FieldStroke : ErrorStroke;

        VisualElement? firstInvalid = null;
        if (!ownershipOk) firstInvalid ??= YesBorder;
        if (!nameOk) firstInvalid ??= NameBorder;
        if (!breedOk) firstInvalid ??= BreedBorder;
        if (!weightOk) firstInvalid ??= WeightBorder;
        if (!vetOk) firstInvalid ??= VetBorder;

        ErrorLabel.IsVisible = firstInvalid is not null;
        return firstInvalid;
    }
}
