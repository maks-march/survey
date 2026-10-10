namespace Survey.Pages;

public partial class ResultsPage : ContentPage
{
	public ResultsPage()
	{
		InitializeComponent();
    }
    private async void GoToMain(object sender, EventArgs e)
    {
        PagesNavigation.GoToAsync(PagesNames.MainPage);
    }
}