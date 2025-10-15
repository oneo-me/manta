using System.Globalization;
using LightKit.I18N.Core;

namespace LightKit.I18N;

public static class Lang
{
    internal static LangSource Source { get; } = new();

    static readonly List<LangDataBase> _items = [];

    public static LangType Default { get; }

    public static LangType Current
    {
        get => Source.Current;
        set => Source.Current = value;
    }

    static Lang()
    {
        Default = Current = LangHelper.GetDefault();
    }

    public static void Register(LangDataBase data)
    {
        lock (_items)
        {
            _items.Insert(0, data);
        }
    }

    static string? GetString(LangType langType, object key)
    {
        lock (_items)
        {
            foreach (var item in _items)
            {
                var result = item.Get(langType, key);
                if (result is not null)
                    return result;
            }
        }

        return null;
    }

    static string NotFoundString(string key)
    {
        return $"![{key}]!";
    }

    static string GetStringCore(LangType langType, object key)
    {
        var result = GetString(langType, key);
        if (result is not null)
            return result;

        if (key is string str)
            return NotFoundString(str);

        if (key is Enum @enum)
            return NotFoundString(@enum.ToString());

        return string.Empty;
    }

    public static string Get(LangType langType, object key, object?[] args)
    {
        var format = GetStringCore(langType, key);
        return string.Format(CultureInfo.InvariantCulture, format, args);
    }

    public static string Get(LangType langType, object key)
    {
        return GetStringCore(langType, key);
    }

    public static string Get(object key, object?[] args)
    {
        return Get(Current, key, args);
    }

    public static string Get(object key)
    {
        return Get(Current, key);
    }
}
