namespace Manta.NodeEditor.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class NodeAttribute : Attribute
{
    public string? DisplayName { get; set; }

    public string Category { get; set; } = "Default";
}
