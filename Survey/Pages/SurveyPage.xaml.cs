using Survey.Services;
using System.Data;
using System.Diagnostics;

namespace Survey.Pages;

public partial class SurveyPage : ContentPage
{
    public static Color SelectedStroke = Color.FromArgb("#B8573C");
    public static Color DefaultStroke = Color.FromArgb("#E7E0D7");
    private SurveyState _state; 
    private readonly Dictionary<Border, (RadioButton Radio, string Ownership, string Hint)> _options;

    public SurveyPage(SurveyState state)
    {
        InitializeComponent();
        _state = state;
        _options = new()
        {
            [YesBorder] = (YesOption, "у вас есть кот или два)", "Сколько у вас котиков?"),
            [PlaningBorder] = (PlaningOption, "вы планируете завести котика)", "Сколько планируете котиков?"),
            [LoveBorder] = (LoveOption, "вы просто любите котиков)", "Сколько бы вы хотели котиков?"),
        };
    }

    private void OnBreedChanged(object sender, EventArgs e)
    {
        _state.CatBreed = (sender as Picker)?.SelectedItem as string;
    }

    private void OnCatNameChanged(object sender, TextChangedEventArgs e)
    {
        _state.CatName = e.NewTextValue;
    }

    private void OnCatAgeChanged(object sender, TextChangedEventArgs e)
    {
        _state.CatAge = int.Parse(e.NewTextValue);
    }

    private void OnTappedCatOption(object sender, TappedEventArgs e)
    {
        if (sender is Border border && _options.TryGetValue(border, out var option))
            option.Radio.IsChecked = true;
    }

    private void OnCatOptionChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value || sender is not RadioButton radio) return;

        var (border, option) = _options.First(pair => pair.Value.Radio == radio);

        foreach (var key in _options.Keys)
            key.Stroke = key == border ? SelectedStroke : DefaultStroke;

        _state.CatOwnership = option.Ownership;
        HintLabel.Text = option.Hint;
    }

    private void OnStepperValueChanged(object sender, EventArgs e)
    {
        _state.CatsCount = (int)((Stepper)sender).Value;
    }
    private async void GoToMain(object sender, EventArgs e)
    {
        PagesNavigation.GoToAsync(PagesNames.MainPage);
    }

    private async void GoToResults(object sender, EventArgs e)
    {
        PagesNavigation.GoToAsync(PagesNames.ResultsPage);
    }
}