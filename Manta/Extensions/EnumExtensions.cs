using System.Globalization;

namespace Manta.Extensions;

public static class EnumExtensions
{
    extension(string value)
    {
        public T ParseEnum<T>(char space) where T : struct, Enum
        {
            var enumerable = value.Split(space).Select(Enum.Parse<T>);
            return (T)Enum.ToObject(typeof(T), enumerable.Select(e => Convert.ToInt32(e, CultureInfo.InvariantCulture)).Aggregate((a, b) => a | b));
        }

        public bool TryParseEnum<T>(out T result, char space = '|') where T : struct, Enum
        {
            try
            {
                result = value.ParseEnum<T>(space);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }

    extension<T>(T @enum) where T : Enum
    {
        public bool HasFlag(T flag)
        {
            var valueInt = Convert.ToInt32(@enum, CultureInfo.InvariantCulture);
            var flagInt = Convert.ToInt32(flag, CultureInfo.InvariantCulture);
            return (valueInt & flagInt) == flagInt;
        }

        public TValue HasFlag<TValue>(T flag, TValue value, TValue defaultValue = default) where TValue : struct
        {
            return @enum.HasFlag<T>(flag) ? value : defaultValue;
        }
    }
}
