using System.Reflection;
using Avalonia.Data;
using Manta.Globalization.Core;
using Manta.Globalization.Markups;

namespace Manta.Globalization;

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
        var langArgs = options.GetArgs();
        if (langArgs is null)
            return langStr;

        return string.Format(Current.ToCultureInfo(), langStr, langArgs);
    }

    public static BindingBase GetBinding(LangGetOptions options)
    {
        var langExtension = new LangExtension(options.LangKey)
        {
            LangType = options.LangType,
            DefValue = options.DefValue ?? options.LangKey
        };

        var langArgs = options.GetArgs();

        if (langArgs is not null)
        {
            if (langArgs.Length > 0)
                langExtension.Data1 = langArgs[0];
            if (langArgs.Length > 1)
                langExtension.Data2 = langArgs[1];
            if (langArgs.Length > 2)
                langExtension.Data3 = langArgs[2];
            if (langArgs.Length > 3)
                langExtension.Data4 = langArgs[3];
            if (langArgs.Length > 4)
                langExtension.Data5 = langArgs[4];
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

        return CreateKey(assemblyName, key);
    }
}
