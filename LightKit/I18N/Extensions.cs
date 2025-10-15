using System.Globalization;
using LightKit.I18N.Core;

namespace LightKit.I18N;

public static class Extensions
{
    public static CultureInfo? ToCultureInfo(this LangType langType)
    {
        if (LangHelper.IsInvariant)
            return null;

        var langMetadata = langType.GetMetadata();

        return LangHelper.Cultures.FirstOrDefault(x => x.Name == langMetadata.SourceType) ?? null;
    }
}
