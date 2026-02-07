namespace Manta.NodeEditor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NodeOutputAttribute : Attribute
{
    public string? DisplayName { get; set; }
}
