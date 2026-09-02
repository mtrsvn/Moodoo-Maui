using Microsoft.Extensions.Logging;
using MoodooApp.Services;
using MoodooApp.ViewModels;
using MoodooApp.Pages;

namespace MoodooApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<MoodService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ChatService>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<MoodViewModel>();
        builder.Services.AddTransient<InsightsViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<MoodDetailViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<MoodLogPage>();
        builder.Services.AddTransient<InsightsPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<MoodDetailPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ChatbotPage>();

        return builder.Build();
    }
}
