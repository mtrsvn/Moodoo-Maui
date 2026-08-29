using MoodooApp.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MoodooApp.Services;

public class MoodService
{
    public ObservableCollection<MoodEntry> Moods { get; private set; } = new();

    public MoodService()
    {
        SeedData();
    }

    public void AddMood(MoodEntry mood)
    {
        Moods.Insert(0, mood);
        SaveMoods();
    }

    private void SaveMoods()
    {
        var json = JsonSerializer.Serialize(Moods);
        Preferences.Default.Set("MoodEntries", json);
    }

    private void SeedData()
    {
        var entries = new List<MoodEntry>
        {
            new() { Mood = "Happy",     Emoji = "😁", Grade = 5, Thoughts = "Had a really productive day! Finished my project early and went for a walk.", Timestamp = DateTime.Now.AddHours(-2) },
            new() { Mood = "Good",      Emoji = "🙂", Grade = 4, Thoughts = "Lunch with a friend was fun. Feeling grateful today.", Timestamp = DateTime.Now.AddDays(-1) },
            new() { Mood = "Neutral",   Emoji = "😐", Grade = 3, Thoughts = "Average day. Nothing special happened.", Timestamp = DateTime.Now.AddDays(-2) },
            new() { Mood = "Happy",     Emoji = "😁", Grade = 5, Thoughts = "Got amazing news from work. Promotion might be coming!", Timestamp = DateTime.Now.AddDays(-3) },
            new() { Mood = "Bad",       Emoji = "😞", Grade = 2, Thoughts = "Didn't sleep well. Headache all day.", Timestamp = DateTime.Now.AddDays(-4) },
            new() { Mood = "Neutral",   Emoji = "😐", Grade = 3, Thoughts = "Work was okay. A bit monotonous.", Timestamp = DateTime.Now.AddDays(-5) },
            new() { Mood = "Good",      Emoji = "🙂", Grade = 4, Thoughts = "Cooked a new recipe tonight and it turned out great.", Timestamp = DateTime.Now.AddDays(-6) },
            new() { Mood = "Terrible",  Emoji = "😫", Grade = 1, Thoughts = "Very stressful day. Too many deadlines.", Timestamp = DateTime.Now.AddDays(-7) },
            new() { Mood = "Happy",     Emoji = "😁", Grade = 5, Thoughts = "Weekend! Went hiking with family.", Timestamp = DateTime.Now.AddDays(-8) },
            new() { Mood = "Good",      Emoji = "🙂", Grade = 4, Thoughts = "Watched a great movie. Relaxing evening.", Timestamp = DateTime.Now.AddDays(-9) },
            new() { Mood = "Neutral",   Emoji = "😐", Grade = 3, Thoughts = "Just a regular Monday.", Timestamp = DateTime.Now.AddDays(-10) },
            new() { Mood = "Bad",       Emoji = "😞", Grade = 2, Thoughts = "Felt anxious about upcoming presentation.", Timestamp = DateTime.Now.AddDays(-11) },
            new() { Mood = "Good",      Emoji = "🙂", Grade = 4, Thoughts = "Presentation went well! Everyone loved it.", Timestamp = DateTime.Now.AddDays(-12) },
            new() { Mood = "Happy",     Emoji = "😁", Grade = 5, Thoughts = "Best day this month. Everything went right.", Timestamp = DateTime.Now.AddDays(-13) },
        };

        Moods = new ObservableCollection<MoodEntry>(entries);
    }
}
