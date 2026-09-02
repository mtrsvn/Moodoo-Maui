using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

[QueryProperty(nameof(LogId), "id")]
public partial class MoodDetailViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    [ObservableProperty]
    private string _logId = string.Empty;

    [ObservableProperty]
    private MoodEntry? _moodEntry;

    [ObservableProperty]
    private string _moodColor = "#7E66EF";

    public ObservableCollection<string> Tags { get; } = new();
    public ObservableCollection<string> Activities { get; } = new();

    public MoodDetailViewModel(MoodService moodService)
    {
        Title = "Mood Details";
        _moodService = moodService;
    }

    partial void OnLogIdChanged(string value)
    {
        LoadLogDetails(value);
    }

    public void LoadLogDetails(string id)
    {
        var entry = _moodService.Moods.FirstOrDefault(m => m.Id == id);
        if (entry != null)
        {
            MoodEntry = entry;
            MoodColor = entry.Grade >= 5 ? "#00E676" : entry.Grade == 4 ? "#7E66EF" : entry.Grade == 3 ? "#FFB800" : "#FF5252";

            Tags.Clear();
            if (entry.Tags != null)
            {
                foreach (var t in entry.Tags) Tags.Add(t);
            }

            Activities.Clear();
            if (entry.Activities != null)
            {
                foreach (var a in entry.Activities) Activities.Add(a);
            }
        }
    }

    [RelayCommand]
    private async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteLogAsync()
    {
        if (MoodEntry == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Delete Log", "Are you sure you want to remove this mood entry?", "Delete", "Cancel");
        if (confirm)
        {
            _moodService.Moods.Remove(MoodEntry);
            await Shell.Current.GoToAsync("..");
        }
    }
}
