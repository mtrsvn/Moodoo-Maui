using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public class CalendarDayItem : ObservableObject
{
    public DateTime Date { get; set; }
    public int DayNumber => Date.Day;
    public string DayName => Date.ToString("ddd");
    public bool IsCurrentMonth { get; set; } = true;
    public bool IsToday { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public ObservableCollection<MoodEntry> DayMoods { get; } = new();
    public bool HasMood => DayMoods.Any();
    public int LogCount => DayMoods.Count;

    public string Emoji
    {
        get
        {
            if (!DayMoods.Any()) return string.Empty;
            if (DayMoods.Count == 1) return DayMoods[0].Emoji;

            double avgGrade = DayMoods.Average(m => m.Grade);
            if (avgGrade >= 4.5) return "😁";
            if (avgGrade >= 3.5) return "🙂";
            if (avgGrade >= 2.5) return "😐";
            if (avgGrade >= 1.5) return "😞";
            return "😫";
        }
    }
}

public partial class HistoryViewModel : BaseViewModel
{
    private readonly MoodService _moodService;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private string _monthTitle = string.Empty;

    [ObservableProperty]
    private CalendarDayItem? _selectedDay;

    [ObservableProperty]
    private bool _hasSelectedDayLogs;

    public ObservableCollection<MoodEntry> SelectedDayLogs { get; } = new();
    public ObservableCollection<CalendarDayItem> DaysInMonth { get; } = new();
    public ObservableCollection<MoodEntry> AllMoods => _moodService.Moods;

    public HistoryViewModel(MoodService moodService)
    {
        Title = "History";
        _moodService = moodService;
        UpdateCalendar(DateTime.Today);
    }

    public void RefreshData()
    {
        UpdateCalendar(SelectedDate);
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        UpdateCalendar(value);
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
    private void SelectDay(CalendarDayItem day)
    {
        if (day == null) return;

        foreach (var d in DaysInMonth)
        {
            d.IsSelected = d == day;
        }

        SelectedDay = day;
        SelectedDayLogs.Clear();
        foreach (var mood in day.DayMoods)
        {
            SelectedDayLogs.Add(mood);
        }
        HasSelectedDayLogs = SelectedDayLogs.Any();
    }

    [RelayCommand]
    private async Task OpenMoodDetailAsync(MoodEntry mood)
    {
        if (mood == null) return;
        await Shell.Current.GoToAsync($"{nameof(Pages.MoodDetailPage)}?id={mood.Id}");
    }

    [RelayCommand]
    private async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void UpdateCalendar(DateTime targetDate)
    {
        MonthTitle = targetDate.ToString("MMMM yyyy");
        DaysInMonth.Clear();

        var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);

        int startOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

        var prevMonth = targetDate.AddMonths(-1);
        int daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
        for (int i = startOffset - 1; i >= 0; i--)
        {
            var date = new DateTime(prevMonth.Year, prevMonth.Month, daysInPrevMonth - i);
            var item = new CalendarDayItem
            {
                Date = date,
                IsCurrentMonth = false,
                IsToday = date.Date == DateTime.Today
            };
            var dayMoods = _moodService.Moods.Where(m => m.Timestamp.Date == date.Date).ToList();
            foreach (var m in dayMoods) item.DayMoods.Add(m);
            DaysInMonth.Add(item);
        }

        CalendarDayItem? todayItem = null;
        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(targetDate.Year, targetDate.Month, day);
            var item = new CalendarDayItem
            {
                Date = date,
                IsCurrentMonth = true,
                IsToday = date.Date == DateTime.Today
            };

            var dayMoods = _moodService.Moods.Where(m => m.Timestamp.Date == date.Date).ToList();
            foreach (var m in dayMoods) item.DayMoods.Add(m);

            if (item.IsToday)
            {
                todayItem = item;
            }

            DaysInMonth.Add(item);
        }

        int totalSoFar = DaysInMonth.Count;
        int remainingDays = (totalSoFar <= 35 ? 35 : 42) - totalSoFar;
        var nextMonth = targetDate.AddMonths(1);
        for (int day = 1; day <= remainingDays; day++)
        {
            var date = new DateTime(nextMonth.Year, nextMonth.Month, day);
            var item = new CalendarDayItem
            {
                Date = date,
                IsCurrentMonth = false,
                IsToday = date.Date == DateTime.Today
            };
            var dayMoods = _moodService.Moods.Where(m => m.Timestamp.Date == date.Date).ToList();
            foreach (var m in dayMoods) item.DayMoods.Add(m);
            DaysInMonth.Add(item);
        }

        CalendarDayItem? matchingItem = DaysInMonth.FirstOrDefault(d => d.Date.Date == targetDate.Date);
        if (matchingItem != null)
        {
            SelectDay(matchingItem);
        }
        else if (todayItem != null)
        {
            SelectDay(todayItem);
        }
        else if (DaysInMonth.Any(d => d.IsCurrentMonth))
        {
            SelectDay(DaysInMonth.First(d => d.IsCurrentMonth));
        }
    }
}
