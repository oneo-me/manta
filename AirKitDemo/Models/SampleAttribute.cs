namespace AirKitDemo.Models;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
sealed class SampleAttribute(SampleGroupType groupType, string name) : Attribute
{
    public SampleGroupType GroupType { get; } = groupType;
    public string Name { get; } = name;
}
