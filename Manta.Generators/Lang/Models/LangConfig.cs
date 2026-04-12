using Manta.Globalization.Core;

namespace Manta.Generators.Lang.Models;

sealed class LangConfig
{
    public LangType Base { get; set; } = LangType.en;
    public LangType Comment { get; set; } = LangType.en;
    public string Modifier { get; set; } = "public";
    public string ClassPrefix { get; set; } = string.Empty;

    public string LangKeyClassName => $"{ClassPrefix}LangKey";
    public string LangDataClassName => $"{ClassPrefix}LangData";
}
