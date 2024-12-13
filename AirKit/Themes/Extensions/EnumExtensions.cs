using System.Globalization;

namespace AirKit.Themes.Extensions;

static class EnumExtensions
{
    public static T Parse<T>(this string value, char space = '|') where T : struct, Enum
    {
        var enumerable = value.Split(space).Select(Enum.Parse<T>);
        return (T)Enum.ToObject(typeof(T), enumerable.Select(e => Convert.ToInt32(e, CultureInfo.InvariantCulture)).Aggregate((a, b) => a | b));
    }

    public static bool HasFlag<T>(this T @enum, T flag) where T : Enum
    {
        var valueInt = Convert.ToInt32(@enum, CultureInfo.InvariantCulture);
        var flagInt = Convert.ToInt32(flag, CultureInfo.InvariantCulture);
        return (valueInt & flagInt) == flagInt;
    }

    public static TValue HasFlag<T, TValue>(this T @enum, T flag, TValue value, TValue defaultValue = default) where T : Enum where TValue : struct
    {
        return @enum.HasFlag<T>(flag) ? value : defaultValue;
    }
}
