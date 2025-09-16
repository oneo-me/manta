namespace Axe.I18N.Core;

public sealed class LangMetadata(LangType type, string englishName, string nativeName, string sourceType)
{
    public LangType Type { get; } = type;
    public string SourceType { get; } = sourceType;
    public string EnglishName { get; } = englishName;
    public string NativeName { get; } = nativeName;
}
