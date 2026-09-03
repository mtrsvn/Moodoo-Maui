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

    [ObservableProperty]
    private string _aiSummaryHeadline = "Balanced & Calm";

    [ObservableProperty]
    private string _aiSummaryEmoji = "✨";

    [ObservableProperty]
    private string _aiSummaryText = "You've had a productive flow with high energy today. Keep resting well tonight!";

    public ObservableCollection<MoodEntry> RecentMoods { get; } = new();

    public HomeViewModel(MoodService moodService, AuthService authService)
    {
        Title = "Home";
        _moodService = moodService;
        _authService = authService;
        
        LoadDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task OpenMoodLogAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.MoodLogPage));
    }

    [RelayCommand]
    private async Task OpenChatbotAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.ChatbotPage));
    }

    [RelayCommand]
    private async Task OpenMoodDetailAsync(MoodEntry mood)
    {
        if (mood == null) return;
        await Shell.Current.GoToAsync($"{nameof(Pages.MoodDetailPage)}?id={mood.Id}");
    }

    [RelayCommand]
    private async Task OpenAboutYourDayAsync()
    {
        // Go to History calendar directly showing today's timeline
        await Shell.Current.GoToAsync(nameof(Pages.HistoryPage));
    }

    [RelayCommand]
    private async Task OpenHistoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.HistoryPage));
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            CurrentUser = _authService.CurrentUser;
            LatestMood = _moodService.Moods.FirstOrDefault();

            RecentMoods.Clear();
            foreach (var mood in _moodService.Moods.Take(3))
            {
                RecentMoods.Add(mood);
            }

            GenerateAiSummary();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void GenerateAiSummary()
    {
        var todayLogs = _moodService.Moods.Where(m => m.Timestamp.Date == DateTime.Today).ToList();
        
        if (!todayLogs.Any())
        {
            AiSummaryEmoji = "🌱";
            AiSummaryHeadline = "Ready for Today";
            AiSummaryText = "No logs recorded yet today. Tap + to check in and start your daily reflection.";
            return;
        }

        double avgGrade = todayLogs.Average(m => m.Grade);
        int logCount = todayLogs.Count;

        if (avgGrade >= 4.3)
        {
            AiSummaryEmoji = "✨";
            AiSummaryHeadline = "Thriving & Uplifted";
            AiSummaryText = logCount > 1 
                ? $"Across {logCount} check-ins, your day was filled with positive momentum, accomplishments, and joy!"
                : "You felt energized and happy today. Carry this wonderful mindset forward!";
        }
        else if (avgGrade >= 3.4)
        {
            AiSummaryEmoji = "🌤️";
            AiSummaryHeadline = "Steady & Grounded";
            AiSummaryText = logCount > 1
                ? $"Your mood stayed steady throughout {logCount} logs today with calm and meaningful moments."
                : "A good and balanced day. You maintained great composure and peace.";
        }
        else if (avgGrade >= 2.4)
        {
            AiSummaryEmoji = "🍃";
            AiSummaryHeadline = "A Bit Mixed";
            AiSummaryText = logCount > 1
                ? $"You navigated highs and lows across {logCount} logs today. Take time tonight to recharge and unwind."
                : "An average day with quiet moments. Take some time for self-care tonight.";
        }
        else
        {
            AiSummaryEmoji = "🌧️";
            AiSummaryHeadline = "A Challenging Day";
            AiSummaryText = "Today felt heavy or stressful. Be gentle with yourself tonight and get plenty of rest.";
        }
    }
}
