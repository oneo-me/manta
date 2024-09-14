namespace Manta.Globalization.Core;

[AttributeUsage(AttributeTargets.Field)]
sealed class LangMetadataAttribute(string emoji, string englishName, string nativeName, string sourceType) : Attribute
{
    public string Emoji { get; } = emoji;
    public string EnglishName { get; } = englishName;
    public string NativeName { get; } = nativeName;
    public string SourceType { get; } = sourceType;
}
