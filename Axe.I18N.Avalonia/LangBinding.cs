using Avalonia.Data;
using Axe.I18N.Avalonia.Markups;
using Axe.I18N.Core;

namespace Axe.I18N.Avalonia;

public static class LangBinding
{
    static IBinding GetCore(LangType? langType, object key, object?[]? args)
    {
        var langExtension = new LangExtension(key);
        if (langType is not null)
            langExtension.LangType = langType.Value;

        if (args is not null)
        {
            if (args.Length > 0)
                langExtension.Data1 = args[0];
            if (args.Length > 1)
                langExtension.Data2 = args[1];
            if (args.Length > 2)
                langExtension.Data3 = args[2];
            if (args.Length > 3)
                langExtension.Data4 = args[3];
            if (args.Length > 4)
                langExtension.Data5 = args[4];
        }

        return langExtension.ToBinding();
    }

    public static IBinding Get(LangType langType, object key, object?[] args)
    {
        return GetCore(langType, key, args);
    }

    public static IBinding Get(LangType langType, object key)
    {
        return GetCore(langType, key, null);
    }

    public static IBinding Get(object key, object?[] args)
    {
        return GetCore(null, key, args);
    }

    public static IBinding Get(object key)
    {
        return GetCore(null, key, null);
    }
}
