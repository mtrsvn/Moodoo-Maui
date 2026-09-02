using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public class MoodOption
{
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public int Grade { get; set; }
    public string ColorHex { get; set; } = "#7E66EF";
    public string Description { get; set; } = string.Empty;
}

public partial class MoodViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    public ObservableCollection<MoodOption> MoodOptions { get; } = new()
    {
        new MoodOption { Name = "Terrible", Emoji = "😫", Grade = 1, ColorHex = "#FF5252", Description = "Overwhelmed or exhausted" },
        new MoodOption { Name = "Bad", Emoji = "😞", Grade = 2, ColorHex = "#FF7E00", Description = "Down or stressed" },
        new MoodOption { Name = "Neutral", Emoji = "😐", Grade = 3, ColorHex = "#FFB800", Description = "Just okay, balanced" },
        new MoodOption { Name = "Good", Emoji = "🙂", Grade = 4, ColorHex = "#7E66EF", Description = "Content and calm" },
        new MoodOption { Name = "Happy", Emoji = "😁", Grade = 5, ColorHex = "#00E676", Description = "Joyful and energetic" }
    };

    [ObservableProperty]
    private string _selectedEmoji = "😁";

    [ObservableProperty]
    private string _selectedMood = "Happy";

    [ObservableProperty]
    private string _selectedDescription = "Joyful and energetic";

    [ObservableProperty]
    private string _selectedColor = "#00E676";

    [ObservableProperty]
    private int _grade = 5;

    [ObservableProperty]
    private string _thoughts = string.Empty;

    public ObservableCollection<SelectableItem> EmotionTags { get; } = new()
    {
        new SelectableItem { Name = "Grateful", Icon = "🙏" },
        new SelectableItem { Name = "Excited", Icon = "🎉" },
        new SelectableItem { Name = "Calm", Icon = "🧘" },
        new SelectableItem { Name = "Productive", Icon = "⚡" },
        new SelectableItem { Name = "Tired", Icon = "😴" },
        new SelectableItem { Name = "Anxious", Icon = "😰" },
        new SelectableItem { Name = "Loved", Icon = "❤️" },
        new SelectableItem { Name = "Motivated", Icon = "🔥" }
    };

    public ObservableCollection<SelectableItem> ActivityFactors { get; } = new()
    {
        new SelectableItem { Name = "Work", Icon = "💼" },
        new SelectableItem { Name = "Exercise", Icon = "🏃" },
        new SelectableItem { Name = "Family", Icon = "👨‍👩‍👧" },
        new SelectableItem { Name = "Friends", Icon = "👥" },
        new SelectableItem { Name = "Sleep", Icon = "🛌" },
        new SelectableItem { Name = "Eating Well", Icon = "🥗" },
        new SelectableItem { Name = "Hobbies", Icon = "🎨" },
        new SelectableItem { Name = "Music", Icon = "🎵" }
    };

    public MoodViewModel(MoodService moodService)
    {
        Title = "Daily Check-in";
        _moodService = moodService;
        SelectMood(MoodOptions.Last());
    }

    [RelayCommand]
    private void SelectMood(MoodOption option)
    {
        if (option == null) return;
        SelectedMood = option.Name;
        SelectedEmoji = option.Emoji;
        Grade = option.Grade;
        SelectedColor = option.ColorHex;
        SelectedDescription = option.Description;
    }

    [RelayCommand]
    private void ToggleTag(SelectableItem item)
    {
        if (item == null) return;
        item.IsSelected = !item.IsSelected;
    }

    [RelayCommand]
    private void ToggleActivity(SelectableItem item)
    {
        if (item == null) return;
        item.IsSelected = !item.IsSelected;
    }

    [RelayCommand]
    private async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task SaveMoodAsync()
    {
        IsBusy = true;
        try
        {
            var selectedTags = EmotionTags.Where(t => t.IsSelected).Select(t => t.Name).ToList();
            var selectedActivities = ActivityFactors.Where(a => a.IsSelected).Select(a => a.Name).ToList();

            var entry = new MoodEntry
            {
                Mood = SelectedMood,
                Emoji = SelectedEmoji,
                Grade = Grade,
                Thoughts = Thoughts,
                Timestamp = DateTime.Now,
                Tags = selectedTags,
                Activities = selectedActivities
            };

            _moodService.AddMood(entry);

            Thoughts = string.Empty;
            foreach (var t in EmotionTags) t.IsSelected = false;
            foreach (var a in ActivityFactors) a.IsSelected = false;

            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("✨ Mood Logged!", $"Your {SelectedMood} mood has been recorded.", "Awesome");
                await Shell.Current.GoToAsync("..");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
