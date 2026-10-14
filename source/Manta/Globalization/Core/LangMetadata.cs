namespace Manta.Globalization.Core;

public sealed class LangMetadata
{
    public LangType Type { get; }
    public string Emoji { get; }
    public string SourceType { get; }
    public string EnglishName { get; }
    public string NativeName { get; }

    internal LangMetadata(LangType type, string emoji, string englishName, string nativeName, string sourceType)
    {
        Type = type;
        Emoji = emoji;
        SourceType = sourceType;
        EnglishName = englishName;
        NativeName = nativeName;
    }
}
