namespace MoodooApp.Models;

public class MoodEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Mood { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public string Thoughts { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int Grade { get; set; }
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
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
}
