using AirKitDemo.Langs;

namespace AirKitDemo.Models;

[AttributeUsage(AttributeTargets.Field)]
sealed class SampleGroupAttribute(LangKey nameKey) : Attribute
{
    public LangKey NameKey { get; } = nameKey;
}
