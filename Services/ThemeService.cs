namespace MoodooApp.Services;

public class ThemeService
{
    public ThemeService()
    {
        Preferences.Default.Remove("IsDarkMode");
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }
    }
}

