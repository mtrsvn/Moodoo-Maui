using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public partial class MoodViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    [ObservableProperty]
    private string _selectedEmoji = "😐";
    
    [ObservableProperty]
    private string _selectedMood = "Neutral";
    
    [ObservableProperty]
    private int _grade = 3;
    
    [ObservableProperty]
    private string _thoughts = string.Empty;

    public ObservableCollection<string> Emojis { get; } = new()
    {
        "😫", "😞", "😐", "🙂", "😁"
    };
    
    public ObservableCollection<string> Moods { get; } = new()
    {
        "Terrible", "Bad", "Neutral", "Good", "Excellent"
    };

    public MoodViewModel(MoodService moodService)
    {
        Title = "Log Mood";
        _moodService = moodService;
    }

    partial void OnGradeChanged(int value)
    {
        if (value >= 1 && value <= 5)
        {
            SelectedEmoji = Emojis[value - 1];
            SelectedMood = Moods[value - 1];
        }
    }

    [RelayCommand]
    private async Task SaveMoodAsync()
    {
        IsBusy = true;
        try
        {
            var entry = new MoodEntry
            {
                Mood = SelectedMood,
                Emoji = SelectedEmoji,
                Grade = Grade,
                Thoughts = Thoughts,
                Timestamp = DateTime.Now
            };
            
            _moodService.AddMood(entry);
            
            await Application.Current!.MainPage!.DisplayAlert("Success", "Your mood has been logged!", "OK");
            Thoughts = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
