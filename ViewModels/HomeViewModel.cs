using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly MoodService _moodService;
    private readonly AuthService _authService;

    [ObservableProperty]
    private User? _currentUser;

    [ObservableProperty]
    private MoodEntry? _latestMood;

    public ObservableCollection<MoodEntry> RecentMoods => _moodService.Moods;

    public HomeViewModel(MoodService moodService, AuthService authService)
    {
        Title = "Home";
        _moodService = moodService;
        _authService = authService;
        
        LoadDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            CurrentUser = _authService.CurrentUser;
            LatestMood = _moodService.Moods.LastOrDefault();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
