using System.Globalization;
using Avalonia.Data.Converters;

namespace ControlsDemo.Converters;

sealed class String2UpperConverter : IValueConverter
{
    public static String2UpperConverter Instance { get; } = new();

    String2UpperConverter()
    {
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
            return str.ToUpper(culture);

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
