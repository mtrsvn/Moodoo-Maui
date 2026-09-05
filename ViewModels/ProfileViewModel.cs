using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MoodooApp.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private User? _currentUser;

    public ProfileViewModel(AuthService authService)
    {
        Title = "Profile";
        _authService = authService;
        
        CurrentUser = _authService.CurrentUser ?? new User();
    }

    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        Application.Current!.MainPage!.DisplayAlert("Logged out", "You have successfully logged out.", "OK");
    }
}
