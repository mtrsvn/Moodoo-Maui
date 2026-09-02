using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public partial class DayMoodStat : ObservableObject
{
    public DateTime Date { get; set; }
    public string DayName { get; set; } = string.Empty;
    public string DayDateText { get; set; } = string.Empty;
    public string Emoji { get; set; } = "😶";
    public string MoodText { get; set; } = "No Entry";
    public string ColorHex { get; set; } = "#7E66EF";
    public bool HasLog { get; set; }
    public int LogCount { get; set; }

    [ObservableProperty]
    private bool _isSelected;
}

public partial class MoodBreakdownItem : ObservableObject
{
    public string MoodLabel { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public double Percentage { get; set; }
    public string PercentageFormatted => $"{Math.Round(Percentage * 100)}%";
    public int DaysCount { get; set; }
    public string DaysText => $"{DaysCount} {(DaysCount == 1 ? "log" : "logs")}";
    public string ProgressColor { get; set; } = "#7E66EF";

    [ObservableProperty]
    private bool _isSelected;
}

public partial class InsightsViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private string _currentMonth = DateTime.Now.ToString("MMMM yyyy");

    [ObservableProperty]
    private string _timeframeTitle = "Last 7 Days";

    [ObservableProperty]
    private string _topMood = "Happy 😁";

    [ObservableProperty]
    private int _positivePercentage = 78;

    [ObservableProperty]
    private int _totalLogsCount = 0;

    [ObservableProperty]
    private string _selectedFilterTitle = "All Recent Logs";

    [ObservableProperty]
    private bool _hasFilteredLogs = true;

    public ObservableCollection<DayMoodStat> WeeklyStats { get; } = new();
    public ObservableCollection<MoodBreakdownItem> MoodBreakdown { get; } = new();
    public ObservableCollection<MoodEntry> FilteredLogs { get; } = new();

    public InsightsViewModel(MoodService moodService)
    {
        Title = "Insights";
        _moodService = moodService;
        CalculateInsightsForMonth(DateTime.Today);
    }

    public void RefreshData()
    {
        CalculateInsightsForMonth(SelectedDate);
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        CalculateInsightsForMonth(value);
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        SelectedDate = SelectedDate.AddMonths(-1);
    }

    [RelayCommand]
    private void NextMonth()
    {
        SelectedDate = SelectedDate.AddMonths(1);
    }

    [RelayCommand]
    private void SelectDayFlow(DayMoodStat dayStat)
    {
        if (dayStat == null) return;

        foreach (var item in WeeklyStats)
        {
            item.IsSelected = item == dayStat;
        }
        foreach (var item in MoodBreakdown)
        {
            item.IsSelected = false;
        }

        var dayEntries = _moodService.Moods.Where(m => m.Timestamp.Date == dayStat.Date.Date).Take(4).ToList();
        FilteredLogs.Clear();
        foreach (var e in dayEntries)
        {
            FilteredLogs.Add(e);
        }

        SelectedFilterTitle = $"{dayStat.DayName}, {dayStat.DayDateText} Logs";
        HasFilteredLogs = FilteredLogs.Any();
    }

    [RelayCommand]
    private void FilterByMood(MoodBreakdownItem breakdown)
    {
        if (breakdown == null) return;

        foreach (var item in MoodBreakdown)
        {
            item.IsSelected = item == breakdown;
        }
        foreach (var item in WeeklyStats)
        {
            item.IsSelected = false;
        }

        var matching = _moodService.Moods
            .Where(m => m.Timestamp.Year == SelectedDate.Year && 
                        m.Timestamp.Month == SelectedDate.Month &&
                        m.Mood.Equals(breakdown.MoodLabel, StringComparison.OrdinalIgnoreCase))
            .Take(4)
            .ToList();

        FilteredLogs.Clear();
        foreach (var e in matching)
        {
            FilteredLogs.Add(e);
        }

        SelectedFilterTitle = $"{breakdown.Emoji} {breakdown.MoodLabel} Logs";
        HasFilteredLogs = FilteredLogs.Any();
    }

    [RelayCommand]
    private async Task OpenHistoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.HistoryPage));
    }

    [RelayCommand]
    private async Task OpenMoodDetailAsync(MoodEntry mood)
    {
        if (mood == null) return;
        await Shell.Current.GoToAsync($"{nameof(Pages.MoodDetailPage)}?id={mood.Id}");
    }

    private void CalculateInsightsForMonth(DateTime targetDate)
    {
        CurrentMonth = targetDate.ToString("MMMM yyyy");
        
        var monthMoods = _moodService.Moods
            .Where(m => m.Timestamp.Year == targetDate.Year && m.Timestamp.Month == targetDate.Month)
            .ToList();

        if (!monthMoods.Any())
        {
            monthMoods = _moodService.Moods.ToList();
        }

        TotalLogsCount = monthMoods.Count;

        if (monthMoods.Any())
        {
            int positiveCount = monthMoods.Count(m => m.Grade >= 4);
            PositivePercentage = (int)Math.Round((double)positiveCount / monthMoods.Count * 100);

            var top = monthMoods.GroupBy(m => m.Mood)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (top != null)
            {
                var sample = top.First();
                TopMood = $"{sample.Mood} {sample.Emoji}".Trim();
            }
        }
        else
        {
            PositivePercentage = 0;
            TopMood = "None";
        }

        // 7-day flow calculation with overall estimated mood
        DateTime endRange = targetDate.Month == DateTime.Today.Month && targetDate.Year == DateTime.Today.Year
            ? DateTime.Today
            : new DateTime(targetDate.Year, targetDate.Month, DateTime.DaysInMonth(targetDate.Year, targetDate.Month));

        WeeklyStats.Clear();
        for (int i = 6; i >= 0; i--)
        {
            var day = endRange.AddDays(-i);
            var dayEntries = _moodService.Moods.Where(m => m.Timestamp.Date == day.Date).ToList();

            if (dayEntries.Any())
            {
                double avgGrade = dayEntries.Average(m => m.Grade);
                string emoji;
                string moodText;
                string color;

                if (avgGrade >= 4.5)
                {
                    emoji = "😁";
                    moodText = "Happy";
                    color = "#00E676";
                }
                else if (avgGrade >= 3.5)
                {
                    emoji = "🙂";
                    moodText = "Good";
                    color = "#7E66EF";
                }
                else if (avgGrade >= 2.5)
                {
                    emoji = "😐";
                    moodText = "Neutral";
                    color = "#FFB800";
                }
                else if (avgGrade >= 1.5)
                {
                    emoji = "😞";
                    moodText = "Bad";
                    color = "#FF5C5C";
                }
                else
                {
                    emoji = "😫";
                    moodText = "Terrible";
                    color = "#FF5252";
                }

                WeeklyStats.Add(new DayMoodStat
                {
                    Date = day,
                    DayName = day.ToString("ddd"),
                    DayDateText = day.ToString("MMM d"),
                    Emoji = emoji,
                    MoodText = moodText,
                    ColorHex = color,
                    HasLog = true,
                    LogCount = dayEntries.Count,
                    IsSelected = i == 0
                });
            }
            else
            {
                WeeklyStats.Add(new DayMoodStat
                {
                    Date = day,
                    DayName = day.ToString("ddd"),
                    DayDateText = day.ToString("MMM d"),
                    Emoji = "—",
                    MoodText = "Untracked",
                    ColorHex = "#6B6B8E",
                    HasLog = false,
                    LogCount = 0,
                    IsSelected = i == 0
                });
            }
        }

        // Breakdown percentages
        MoodBreakdown.Clear();
        int total = monthMoods.Count;
        if (total > 0)
        {
            int happy = monthMoods.Count(m => m.Grade == 5);
            int good = monthMoods.Count(m => m.Grade == 4);
            int neutral = monthMoods.Count(m => m.Grade == 3);
            int low = monthMoods.Count(m => m.Grade <= 2);

            MoodBreakdown.Add(new MoodBreakdownItem { MoodLabel = "Happy", Emoji = "😁", Percentage = (double)happy / total, DaysCount = happy, ProgressColor = "#00E676" });
            MoodBreakdown.Add(new MoodBreakdownItem { MoodLabel = "Good", Emoji = "🙂", Percentage = (double)good / total, DaysCount = good, ProgressColor = "#7E66EF" });
            MoodBreakdown.Add(new MoodBreakdownItem { MoodLabel = "Neutral", Emoji = "😐", Percentage = (double)neutral / total, DaysCount = neutral, ProgressColor = "#FFB800" });
            MoodBreakdown.Add(new MoodBreakdownItem { MoodLabel = "Low", Emoji = "😞", Percentage = (double)low / total, DaysCount = low, ProgressColor = "#FF5C5C" });
        }

        // Populate initial filtered logs for today
        var initialEntries = _moodService.Moods.Where(m => m.Timestamp.Date == DateTime.Today).ToList();
        if (!initialEntries.Any() && monthMoods.Any())
        {
            initialEntries = monthMoods.Take(4).ToList();
        }

        FilteredLogs.Clear();
        foreach (var entry in initialEntries)
        {
            FilteredLogs.Add(entry);
        }
        SelectedFilterTitle = "Today's Check-ins";
        HasFilteredLogs = FilteredLogs.Any();
    }
}
