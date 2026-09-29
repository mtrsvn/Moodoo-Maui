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

        _ = LoadUserAsync();
    }

    private async Task LoadUserAsync()
    {
        try
        {
            CurrentUser = await _authService.GetUserAsync("test@test.com");

            if (CurrentUser == null)
            {
                CurrentUser = new User
                {
                    Uid = "test-user-001",
                    Username = "TestUser",
                    Email = "test@test.com",
                    FullName = "Test User",
                    IsVerified = true,
                    CreatedAt = DateTime.Now
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Failed to load profile user: {ex.Message}"
            );

            CurrentUser = new User
            {
                Uid = "test-user-001",
                Username = "TestUser",
                Email = "test@test.com",
                FullName = "Test User",
                IsVerified = true,
                CreatedAt = DateTime.Now
            };
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        _authService.Logout();

        await Application.Current!.Windows[0].Page!
            .DisplayAlertAsync(
                "Logged out",
                "You have successfully logged out.",
                "OK"
            );
    }
}
