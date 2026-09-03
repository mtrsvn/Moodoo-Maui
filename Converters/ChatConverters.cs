using System.Globalization;

namespace MoodooApp.Converters;

public class SenderToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var sender = value as string;
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        if (sender == "user") 
            return isDark ? Color.FromArgb("#38325E") : Color.FromArgb("#7E66EF");
        
        // AI / Bot bubble - subtle container like ChatGPT / Gemini
        return isDark ? Color.FromArgb("#1E1E2D") : Color.FromArgb("#F3F4F6");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class SenderToTextColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var sender = value as string;
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        if (sender == "user") 
            return Colors.White;
        
        return isDark ? Color.FromArgb("#E2E8F0") : Color.FromArgb("#1E293B");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class SenderToAlignmentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var sender = value as string;
        return sender == "user" ? LayoutOptions.End : LayoutOptions.Start;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
