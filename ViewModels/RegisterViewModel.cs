using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodooApp.Models;
using MoodooApp.Services;

namespace MoodooApp.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public RegisterViewModel(DatabaseService databaseService)
    {
        Title = "Create Account";
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Please fill in all fields.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        var existingUser =
            await _databaseService.GetUserByEmailAsync(Email.Trim());

        if (existingUser != null)
        {
            ErrorMessage = "An account with this email already exists.";
            return;
        }

        var user = new User
        {
            Uid = Guid.NewGuid().ToString(),
            Username = Username.Trim(),
            Email = Email.Trim(),
            Password = Password,
            FullName = FullName.Trim(),
            IsVerified = false,
            CreatedAt = DateTime.Now
        };

        await _databaseService.SaveUserAsync(user);

        await Shell.Current.DisplayAlertAsync(
            "Account Created",
            "Your account has been created successfully. You can now log in.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task BackToLoginAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
