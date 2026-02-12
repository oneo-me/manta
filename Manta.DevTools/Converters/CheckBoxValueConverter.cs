using System.Globalization;
using Avalonia.Data.Converters;

namespace Manta.DevTools.Converters;

sealed class CheckBoxValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool bol)
            return bol ? "True" : "False";

        return "Null";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
