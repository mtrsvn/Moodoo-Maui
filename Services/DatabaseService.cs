using SQLite;
using MoodooApp.Models;

namespace MoodooApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    private string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, "Moodoo.db");

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        if (!File.Exists(DatabasePath))
        {
            await using var sourceStream =
                await FileSystem.OpenAppPackageFileAsync("Moodoo.db");

            await using var destinationStream =
                File.Create(DatabasePath);

            await sourceStream.CopyToAsync(destinationStream);
        }

        _database = new SQLiteAsyncConnection(DatabasePath);
    }

    public async Task<List<string>> GetTableNamesAsync()
    {
        if (_database == null)
            await InitializeAsync();

        var tables = await _database!.QueryAsync<TableInfo>(
            "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;"
        );

        return tables.Select(t => t.Name).ToList();
    }

    public async Task<User?> GetUserByUidAsync(string uid)
    {
        if (_database == null)
            await InitializeAsync();

        return await _database!.Table<User>()
            .Where(u => u.Uid == uid)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        if (_database == null)
            await InitializeAsync();

        return await _database!.Table<User>()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        if (_database == null)
            await InitializeAsync();

        return await _database!.Table<User>().ToListAsync();
    }

    public async Task SaveUserAsync(User user)
    {
        if (_database == null)
            await InitializeAsync();

        var existingUser = await GetUserByUidAsync(user.Uid);

        if (existingUser == null)
        {
            await _database!.InsertAsync(user);
        }
        else
        {
            await _database!.UpdateAsync(user);
        }
    }

    private class TableInfo
    {
        public string Name { get; set; } = string.Empty;
    }
}
