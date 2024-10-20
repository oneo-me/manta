using System.Globalization;
using AirKit.Themes.Base;
using Avalonia.Data.Converters;

namespace AirKit.Themes.Converters;

sealed class ThemeValueConverter : IMultiValueConverter
{
    public static ThemeValueConverter Instance { get; } = new();

    ThemeValueConverter()
    {
    }

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not ThemeValueBase themeValue)
            throw new ArgumentException("Invalid parameter type", nameof(parameter));

        if (themeValue.IsDynamic)
            return themeValue.OnConvert(values);

        if (themeValue.Values is null)
            throw new InvalidOperationException("Static theme value is not initialized");

        return themeValue.OnConvert(themeValue.Values);
    }
}
