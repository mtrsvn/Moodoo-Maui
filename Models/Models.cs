namespace MoodooApp.Models;

public class MoodEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Mood { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public string Thoughts { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int Grade { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Activities { get; set; } = new();
}

public class SelectableItem : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}

public class ActivityItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; } = string.Empty;
    public string Sender { get; set; } = "user";
    public DateTime Timestamp { get; set; } = DateTime.Now;
}

public class User
{
    public string Uid { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = "Mark";
    public string Email { get; set; } = "mark@moodoo.app";
    public string FullName { get; set; } = "Mark Developer";
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Today.AddMonths(-2).AddDays(-14);
    public string JoinedDateFormatted => $"Joined {CreatedAt:MMMM yyyy}";
}
