using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public class DayMoodStat
{
    public string DayName { get; set; } = string.Empty;
    public string Emoji { get; set; } = "😶";
    public double ScoreHeight { get; set; } = 20;
    public string ColorHex { get; set; } = "#7E66EF";
}

public partial class InsightsViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    [ObservableProperty]
    private string _currentMonth = DateTime.Now.ToString("MMMM yyyy");

    [ObservableProperty]
    private double _averageScore = 4.2;

    [ObservableProperty]
    private string _topMood = "Happy 😁";

    [ObservableProperty]
    private int _positivePercentage = 78;

    public ObservableCollection<DayMoodStat> WeeklyStats { get; } = new()
    {
        new DayMoodStat { DayName = "Mon", Emoji = "🙂", ScoreHeight = 80, ColorHex = "#7E66EF" },
        new DayMoodStat { DayName = "Tue", Emoji = "😁", ScoreHeight = 100, ColorHex = "#00E676" },
        new DayMoodStat { DayName = "Wed", Emoji = "😐", ScoreHeight = 60, ColorHex = "#FFB800" },
        new DayMoodStat { DayName = "Thu", Emoji = "😁", ScoreHeight = 100, ColorHex = "#00E676" },
        new DayMoodStat { DayName = "Fri", Emoji = "🙂", ScoreHeight = 85, ColorHex = "#7E66EF" },
        new DayMoodStat { DayName = "Sat", Emoji = "😁", ScoreHeight = 110, ColorHex = "#00E676" },
        new DayMoodStat { DayName = "Sun", Emoji = "😁", ScoreHeight = 120, ColorHex = "#00E676" },
    };

    public ObservableCollection<MoodEntry> RecentHistory => _moodService.Moods;

    public InsightsViewModel(MoodService moodService)
    {
        Title = "Insights";
        _moodService = moodService;
    }
}
