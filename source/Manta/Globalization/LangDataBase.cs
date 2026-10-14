using Manta.Globalization.Core;

namespace Manta.Globalization;

public abstract class LangDataBase(LangType defCulture)
{
    protected abstract string? GetOverride(LangType langType, string key);

    public string? Get(LangType langType, string key, bool auto)
    {
        var result = GetOverride(langType, key);
        if (result is not null)
            return result;

        if (langType == defCulture)
            return null;

        if (auto)
            return GetOverride(defCulture, key);

        return null;
    }
}
