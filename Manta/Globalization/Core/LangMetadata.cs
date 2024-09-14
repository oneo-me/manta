namespace Manta.Globalization.Core;

public sealed class LangMetadata(LangType type, string emoji, string englishName, string nativeName, string sourceType)
{
    public LangType Type { get; } = type;
    public string Emoji { get; } = emoji;
    public string SourceType { get; } = sourceType;
    public string EnglishName { get; } = englishName;
    public string NativeName { get; } = nativeName;
}
