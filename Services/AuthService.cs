using MoodooApp.Models;

namespace MoodooApp.Services;

public class AuthService
{
    public User? CurrentUser { get; private set; } = new User();

    public bool IsAuthenticated => CurrentUser != null;

    public async Task<bool> LoginAsync(string email, string password)
    {
        await Task.Delay(500);
        if (email == "test@test.com" && password == "password")
        {
            CurrentUser = new User { Email = email, Username = "TestUser", FullName = "Test User" };
            return true;
        }
        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
