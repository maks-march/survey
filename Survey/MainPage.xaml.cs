using Survey.Pages;

namespace Survey
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void GoToSurvey(object sender, EventArgs e)
        {
            PagesNavigation.GoToAsync(PagesNames.SurveyPage);
        }
    }
}
