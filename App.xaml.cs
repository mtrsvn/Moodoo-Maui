using MoodooApp.Services;

namespace MoodooApp;

public partial class App : Application
{
    public App(DatabaseService databaseService)
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light;

        _ = InitializeAppAsync(databaseService);
    }

    private async Task InitializeAppAsync(
        DatabaseService databaseService)
    {
        try
        {
            await databaseService.InitializeAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"App initialization failed: {ex.Message}"
            );
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
