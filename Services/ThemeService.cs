using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoodooApp.Services;

public class ThemeService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private bool _isDark;
    public bool IsDark
    {
        get => _isDark;
        set
        {
            if (_isDark != value)
            {
                _isDark = value;
                Preferences.Default.Set("IsDarkMode", value);
                ApplyTheme();
                OnPropertyChanged();
            }
        }
    }

    public ThemeService()
    {
        IsDark = Preferences.Default.Get("IsDarkMode", true);
        ApplyTheme();
    }

    public void ToggleTheme()
    {
        IsDark = !IsDark;
    }

    private void ApplyTheme()
    {
        Application.Current!.UserAppTheme = IsDark ? AppTheme.Dark : AppTheme.Light;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
