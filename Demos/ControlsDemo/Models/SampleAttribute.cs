using ControlsDemo.Langs;

namespace ControlsDemo.Models;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
sealed class SampleAttribute(SampleGroupType groupType, LangKey nameKey) : Attribute
{
    public SampleGroupType GroupType { get; } = groupType;
    public LangKey NameKey { get; } = nameKey;
}
