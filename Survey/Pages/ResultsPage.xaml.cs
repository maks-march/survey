using Survey.Services;

namespace Survey.Pages;

public partial class ResultsPage : ContentPage
{
    private readonly SurveyState _state;

    public ResultsPage(SurveyState state)
    {
        InitializeComponent();
        _state = state;
        ShowResults();
    }

    private void ShowResults()
    {
        string name = string.IsNullOrWhiteSpace(_state.CatName) ? "Ваш котик" : _state.CatName.Trim();
        string breed = string.IsNullOrWhiteSpace(_state.CatBreed) ? "порода не выбрана" : _state.CatBreed;
        string weight = _state.CatMass > 0 ? $"{_state.CatMass} кг" : "вес не указан";
        string date = _state.CatDate.Year > 2000 ? _state.CatDate.ToString("dd MMMM yyyy") : "дата не указана";

        SummaryLabel.Text = $"{name}: {breed}, {weight}. Появился у вас {date}.";

        AddRow("Отношение к котикам", Capitalize(_state.CatOwnership) ?? "не указано");
        AddRow("Сколько котов у вас", _state.CatsCount is > 0 ? _state.CatsCount.ToString() : "не указано");
        AddRow("Характер", _state.CatTraits.Count > 0
            ? Capitalize(string.Join(", ", _state.CatTraits))!
            : "не отмечен");
        AddRow("Здоровье", _state.CatHealthy
            ? "регулярно посещает ветеринара"
            : "к ветеринару не ходит");
        AddRow("Что ещё хочется рассказать", string.IsNullOrWhiteSpace(_state.CatInfo) ? "ничего" : _state.CatInfo.Trim());
    }

    private void AddRow(string title, string value)
    {
        RowsStack.Children.Add(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                new Label { Text = title, FontSize = 13, TextColor = Color.FromArgb("#79716A") },
                new Label { Text = value, FontSize = 16, TextColor = Color.FromArgb("#302C29"), FontFamily = "InterRegular" },
            }
        });
    }

    private static string? Capitalize(string? text) =>
        string.IsNullOrEmpty(text) ? text : char.ToUpper(text[0]) + text[1..];

    private async void GoToMain(object sender, EventArgs e)
    {
        _state.Reset();
        PagesNavigation.GoToAsync(PagesNames.MainPage);
    }
}
