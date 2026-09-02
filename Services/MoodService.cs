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
            // TODAY: Multiple entries (Morning, Afternoon, Evening)
            new() {
                Mood = "Happy",
                Emoji = "😁",
                Grade = 5,
                Thoughts = "Finished all tasks and had an awesome workout session!",
                Timestamp = DateTime.Today.AddHours(20).AddMinutes(30),
                Tags = new() { "Productive", "Excited", "Motivated" },
                Activities = new() { "Work", "Exercise", "Hobbies" }
            },
            new() {
                Mood = "Good",
                Emoji = "🙂",
                Grade = 4,
                Thoughts = "Had delicious lunch with colleagues. Work is going smoothly.",
                Timestamp = DateTime.Today.AddHours(13).AddMinutes(15),
                Tags = new() { "Grateful", "Calm" },
                Activities = new() { "Work", "Eating Well", "Friends" }
            },
            new() {
                Mood = "Neutral",
                Emoji = "😐",
                Grade = 3,
                Thoughts = "Morning routine felt a bit slow and sleepy.",
                Timestamp = DateTime.Today.AddHours(8).AddMinutes(45),
                Tags = new() { "Tired" },
                Activities = new() { "Sleep" }
            },

            // YESTERDAY: Multiple entries
            new() {
                Mood = "Happy",
                Emoji = "😁",
                Grade = 5,
                Thoughts = "Family dinner was lovely. Watched movie together.",
                Timestamp = DateTime.Today.AddDays(-1).AddHours(21),
                Tags = new() { "Loved", "Grateful" },
                Activities = new() { "Family", "Hobbies", "Eating Well" }
            },
            new() {
                Mood = "Bad",
                Emoji = "😞",
                Grade = 2,
                Thoughts = "Traffic was stressful and had a slight headache.",
                Timestamp = DateTime.Today.AddDays(-1).AddHours(14),
                Tags = new() { "Anxious", "Tired" },
                Activities = new() { "Work" }
            },
            new() {
                Mood = "Good",
                Emoji = "🙂",
                Grade = 4,
                Thoughts = "Good morning walk in the neighborhood.",
                Timestamp = DateTime.Today.AddDays(-1).AddHours(7).AddMinutes(30),
                Tags = new() { "Calm", "Motivated" },
                Activities = new() { "Exercise" }
            },

            // 2 DAYS AGO: Multiple entries
            new() {
                Mood = "Good",
                Emoji = "🙂",
                Grade = 4,
                Thoughts = "Cooked a nice dinner and listened to music.",
                Timestamp = DateTime.Today.AddDays(-2).AddHours(19),
                Tags = new() { "Calm", "Grateful" },
                Activities = new() { "Music", "Eating Well" }
            },
            new() {
                Mood = "Happy",
                Emoji = "😁",
                Grade = 5,
                Thoughts = "Big feature release deployed with zero bugs!",
                Timestamp = DateTime.Today.AddDays(-2).AddHours(11),
                Tags = new() { "Productive", "Excited" },
                Activities = new() { "Work" }
            },

            // 3 DAYS AGO
            new() {
                Mood = "Happy",
                Emoji = "😁",
                Grade = 5,
                Thoughts = "Weekend hike with friends. Weather was amazing!",
                Timestamp = DateTime.Today.AddDays(-3).AddHours(16),
                Tags = new() { "Excited", "Grateful" },
                Activities = new() { "Friends", "Exercise" }
            },
            new() {
                Mood = "Good",
                Emoji = "🙂",
                Grade = 4,
                Thoughts = "Early morning coffee and journaling.",
                Timestamp = DateTime.Today.AddDays(-3).AddHours(9),
                Tags = new() { "Calm" },
                Activities = new() { "Hobbies" }
            },

            // 4 DAYS AGO
            new() {
                Mood = "Neutral",
                Emoji = "😐",
                Grade = 3,
                Thoughts = "Routine chores and cleaning around the house.",
                Timestamp = DateTime.Today.AddDays(-4).AddHours(15),
                Tags = new() { "Tired" },
                Activities = new() { "Work" }
            },

            // 5 DAYS AGO
            new() {
                Mood = "Good",
                Emoji = "🙂",
                Grade = 4,
                Thoughts = "Read 50 pages of my new book.",
                Timestamp = DateTime.Today.AddDays(-5).AddHours(20),
                Tags = new() { "Calm" },
                Activities = new() { "Hobbies" }
            },

            // 6 DAYS AGO
            new() {
                Mood = "Happy",
                Emoji = "😁",
                Grade = 5,
                Thoughts = "Great team meeting and productive brainstorm session.",
                Timestamp = DateTime.Today.AddDays(-6).AddHours(14),
                Tags = new() { "Motivated", "Productive" },
                Activities = new() { "Work" }
            }
        };

        Moods = new ObservableCollection<MoodEntry>(entries);
    }
}
