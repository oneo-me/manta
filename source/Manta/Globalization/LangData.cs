using Manta.Globalization.Core;

namespace Manta.Globalization;

public sealed class LangData(LangType defCulture) : LangDataBase(defCulture)
{
    readonly Dictionary<LangType, Dictionary<string, string>> _items = new();

    protected override string? GetOverride(LangType langType, string key)
    {
        if (_items.TryGetValue(langType, out var item))
            if (item.TryGetValue(key, out var value))
                return value;

        return null;
    }

    public void Add(LangType langType, string key, string value)
    {
        if (_items.TryGetValue(langType, out var values) is false)
            _items[langType] = values = [];

        values[key] = value;
    }
}
