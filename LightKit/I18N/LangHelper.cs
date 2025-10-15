using System.Globalization;
using LightKit.I18N.Core;

namespace LightKit.I18N;

static class LangHelper
{
    public static CultureInfo[] Cultures { get; } = CultureInfo.GetCultures(CultureTypes.AllCultures);
    public static bool IsInvariant { get; } = Cultures.Length <= 0 || (Cultures.Length == 1 && string.IsNullOrWhiteSpace(Cultures[0].Name));

    public static LangType GetDefault()
    {
        if (IsInvariant)
            return LangType.en;

        var current = new List<string>();
        var currentInfo = CultureInfo.CurrentUICulture;
        while (string.IsNullOrWhiteSpace(currentInfo.Name) is false)
        {
            current.Add(currentInfo.Name.Replace("-", "_", StringComparison.Ordinal));
            currentInfo = currentInfo.Parent;
        }

        var allMetadata = Enum.GetValues<LangType>().Select(x => x.GetMetadata()).ToList();

        foreach (var item in allMetadata)
            if (current.Contains(item.SourceType))
                return item.Type;

        return LangType.en;
    }
}
