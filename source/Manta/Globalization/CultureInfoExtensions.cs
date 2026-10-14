using System.Globalization;
using Manta.Globalization.Core;

namespace Manta.Globalization;

public static class CultureInfoExtensions
{
    public static CultureInfo? ToCultureInfo(this LangType langType)
    {
        if (LangHelper.IsInvariant)
            return null;

        var langMetadata = langType.GetMetadata();

        return LangHelper.Cultures.FirstOrDefault(x => x.Name == langMetadata.SourceType) ?? null;
    }
}
