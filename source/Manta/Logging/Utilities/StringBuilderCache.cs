using System.Text;

namespace Manta.Logging.Utilities;

static class StringBuilderCache
{
    const int MaxBuilderSize = 360;

    [ThreadStatic]
    static StringBuilder? _cachedInstance;

    public static StringBuilder Acquire(int capacity = 16)
    {
        if (capacity <= MaxBuilderSize)
        {
            var sb = _cachedInstance;
            if (sb != null)
                if (capacity <= sb.Capacity)
                {
                    _cachedInstance = null;
                    sb.Clear();
                    return sb;
                }
        }

        return new(capacity);
    }

    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= MaxBuilderSize)
            _cachedInstance = sb;
    }

    public static string GetStringAndRelease(StringBuilder sb)
    {
        var result = sb.ToString();
        Release(sb);
        return result;
    }
}
