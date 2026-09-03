using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MoodooApp.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    private readonly ThemeService _themeService;

    [ObservableProperty]
    private User? _currentUser;

    public bool IsDarkTheme
    {
        get => _themeService.IsDark;
        set => _themeService.IsDark = value;
    }

    public ProfileViewModel(AuthService authService, ThemeService themeService)
    {
        Title = "Profile";
        _authService = authService;
        _themeService = themeService;
        
        CurrentUser = _authService.CurrentUser ?? new User();
    }

    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        Application.Current!.MainPage!.DisplayAlert("Logged out", "You have successfully logged out.", "OK");
    }
}
