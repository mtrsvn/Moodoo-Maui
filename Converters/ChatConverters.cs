using System.Globalization;

namespace MoodooApp.Converters;

public class SenderToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var sender = value as string;
        if (sender == "user") 
            return Color.FromArgb("#7E66EF");
        
        return Application.Current!.RequestedTheme == AppTheme.Dark 
            ? Color.FromArgb("#252538") 
            : Color.FromArgb("#E5E5EA");
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
