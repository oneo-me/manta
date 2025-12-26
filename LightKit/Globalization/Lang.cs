using System.Reflection;
using Avalonia.Data;
using LightKit.Globalization.Core;
using LightKit.Globalization.Markups;

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

    static string GetString(LangType langType, string key, string def)
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

        return def;
    }

    public static string Get(LangGetOptions options)
    {
        var langType = options.LangType ?? Current;
        var langStr = GetString(langType, options.LangKey, options.DefValue ?? options.LangKey);
        if (options.Args is null)
            return langStr;

        return string.Format(Current.ToCultureInfo(), langStr, options.Args);
    }

    public static IBinding GetBinding(LangGetOptions options)
    {
        var langExtension = new LangExtension(options.LangKey)
        {
            LangType = options.LangType,
            DefValue = options.DefValue ?? options.LangKey
        };

        if (options.Args is not null)
        {
            if (options.Args.Length > 0)
                langExtension.Data1 = options.Args[0];
            if (options.Args.Length > 1)
                langExtension.Data2 = options.Args[1];
            if (options.Args.Length > 2)
                langExtension.Data3 = options.Args[2];
            if (options.Args.Length > 3)
                langExtension.Data4 = options.Args[3];
            if (options.Args.Length > 4)
                langExtension.Data5 = options.Args[4];
        }

        return langExtension.ToBinding();
    }

    public static string CreateKey(string assemblyName, string key)
    {
        return $"{assemblyName.Replace(".", "_")}_{key}";
    }

    public static string CreateKey(Assembly assembly, string key)
    {
        var assemblyName = assembly.GetName().Name;
        if (assemblyName is null)
            throw new InvalidOperationException("Assembly name is null.");

        return $"{assemblyName}_{key}";
    }
}
