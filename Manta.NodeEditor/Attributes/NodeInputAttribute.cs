namespace Manta.NodeEditor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NodeInputAttribute : Attribute
{
    public string? DisplayName { get; set; }

    public int Order { get; set; }
}
