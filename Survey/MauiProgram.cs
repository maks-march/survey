using Microsoft.Extensions.Logging;
using Survey.Pages;
using Survey.Services;

namespace Survey
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter-Italic.ttf", "InterItalic");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<SurveyState>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            Routing.RegisterRoute(PagesNames.SurveyPage.ToString(), typeof(SurveyPage));
            Routing.RegisterRoute(PagesNames.ResultsPage.ToString(), typeof(ResultsPage));
            Routing.RegisterRoute(PagesNames.MainPage.ToString(), typeof(MainPage));
            return builder.Build();
        }
    }
}
