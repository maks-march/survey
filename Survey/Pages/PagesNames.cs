namespace Survey.Pages
{
    public enum PagesNames
    {
        MainPage,
        SurveyPage,
        ResultsPage
    }

    public static class PagesNavigation
    {
        private static bool _isBlocked = false;
        public static async void GoToAsync(PagesNames name)
        {
            if (_isBlocked)
                return;
            _isBlocked = !_isBlocked;
            string address = name switch
            {
                PagesNames.MainPage => "//" + name.ToString(),
                _ => name.ToString()
            };
            await Shell.Current.GoToAsync(address);
            _isBlocked = !_isBlocked;
        }
    }
}
