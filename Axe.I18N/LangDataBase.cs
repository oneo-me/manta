using Axe.I18N.Core;

namespace Axe.I18N;

public abstract class LangDataBase(LangType? defCulture)
{
    protected abstract string? GetOverride(LangType langType, object key);

    public string? Get(LangType langType, object key)
    {
        var result = GetOverride(langType, key);
        if (result is not null)
            return result;

        if (langType == defCulture)
            return null;

        if (defCulture is null)
            return null;

        return GetOverride(defCulture.Value, key);
    }
}
