using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodooApp.Services;

namespace MoodooApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public LoginViewModel(AuthService authService)
    {
        Title = "Login";
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter your email and password.";
            return;
        }

        var success = await _authService.LoginAsync(
            Email.Trim(),
            Password
        );

        if (!success)
        {
            ErrorMessage = "Invalid email or password.";
            return;
        }

        await Shell.Current.GoToAsync("//MainTabs/HomePage");
    }

    [RelayCommand]
    private async Task SignUpAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.RegisterPage));
    }
}
