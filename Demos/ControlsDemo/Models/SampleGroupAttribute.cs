using ControlsDemo.Langs;

namespace ControlsDemo.Models;

[AttributeUsage(AttributeTargets.Field)]
sealed class SampleGroupAttribute(LangKey nameKey) : Attribute
{
    public LangKey NameKey { get; } = nameKey;
}
