using MoodooApp.Models;

namespace MoodooApp.Services;

public class AuthService
{
    private readonly DatabaseService _databaseService;

    public User? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser != null;

    public AuthService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task InitializeAsync()
    {
        CurrentUser = await _databaseService.GetUserByEmailAsync("test@test.com");
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await _databaseService.GetUserByEmailAsync(email);
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        await Task.Delay(300);

        var user = await _databaseService.GetUserByEmailAsync(email.Trim());

        if (user == null)
            return false;

        if (user.Password != password)
            return false;

        CurrentUser = user;

        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
