using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace LightKit.Converters;

public sealed class Number2ThicknessConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int i)
            return new Thickness(i);
        if (value is double d)
            return new Thickness(d);
        if (value is float f)
            return new Thickness(f);
        if (value is long l)
            return new Thickness(l);
        if (value is short s)
            return new Thickness(s);
        if (value is byte b)
            return new Thickness(b);

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
