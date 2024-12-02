namespace AirKitDemo.Models;

[AttributeUsage(AttributeTargets.Field)]
sealed class SampleGroupAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
