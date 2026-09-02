using System.Globalization;

namespace MoodooApp.Converters;

public class BoolToColorConverter : BindableObject, IValueConverter
{
    public static readonly BindableProperty TrueColorProperty =
        BindableProperty.Create(nameof(TrueColor), typeof(Color), typeof(BoolToColorConverter), Colors.Transparent);

    public static readonly BindableProperty FalseColorProperty =
        BindableProperty.Create(nameof(FalseColor), typeof(Color), typeof(BoolToColorConverter), Colors.Transparent);

    public Color TrueColor
    {
        get => (Color)GetValue(TrueColorProperty);
        set => SetValue(TrueColorProperty, value);
    }

    public Color FalseColor
    {
        get => (Color)GetValue(FalseColorProperty);
        set => SetValue(FalseColorProperty, value);
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return TrueColor;
        return FalseColor;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class BoolToTextColorConverter : BindableObject, IValueConverter
{
    public static readonly BindableProperty TrueTextColorProperty =
        BindableProperty.Create(nameof(TrueTextColor), typeof(Color), typeof(BoolToTextColorConverter), Colors.White);

    public static readonly BindableProperty FalseTextColorProperty =
        BindableProperty.Create(nameof(FalseTextColor), typeof(Color), typeof(BoolToTextColorConverter), Color.FromArgb("#A0A0B8"));

    public Color TrueTextColor
    {
        get => (Color)GetValue(TrueTextColorProperty);
        set => SetValue(TrueTextColorProperty, value);
    }

    public Color FalseTextColor
    {
        get => (Color)GetValue(FalseTextColorProperty);
        set => SetValue(FalseTextColorProperty, value);
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return TrueTextColor;
        return FalseTextColor;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class NullToBoolConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool hasValue = value != null;
        return Invert ? !hasValue : hasValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class BoolToDoubleConverter : IValueConverter
{
    public double TrueValue { get; set; } = 1.0;
    public double FalseValue { get; set; } = 0.35;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return TrueValue;
        return FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
