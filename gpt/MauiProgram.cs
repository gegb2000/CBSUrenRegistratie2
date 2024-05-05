using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Logging;
using CBSUrenRegistratie2.Services;
namespace CBSUrenRegistratie2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();


#endif
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            return builder.Build();
        }
    }
}
