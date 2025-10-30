using LightKit.Globalization.Core;

namespace LightKit.Globalization;

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

    static string? GetString(LangType langType, string key)
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

    static string GetStringCore(LangType langType, string key)
    {
        var result = GetString(langType, key);
        if (result is not null)
            return result;

        return key;
    }

    public static string Get(LangType langType, string key, object?[] args)
    {
        var format = GetStringCore(langType, key);
        return string.Format(Current.ToCultureInfo(), format, args);
    }

    public static string Get(LangType langType, string key)
    {
        return GetStringCore(langType, key);
    }

    public static string Get(string key, object?[] args)
    {
        return Get(Current, key, args);
    }

    public static string Get(string key)
    {
        return Get(Current, key);
    }
}
