namespace Axe.I18N.Core;

[AttributeUsage(AttributeTargets.Field)]
sealed class LangMetadataAttribute(string englishName, string nativeName, string sourceType) : Attribute
{
    public string EnglishName { get; } = englishName;
    public string NativeName { get; } = nativeName;
    public string SourceType { get; } = sourceType;
}
