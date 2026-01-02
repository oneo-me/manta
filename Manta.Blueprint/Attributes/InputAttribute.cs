namespace Manta.Blueprint.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class InputAttribute : Attribute
{
    public string? DisplayName { get; set; }
    public int Order { get; set; }
}
