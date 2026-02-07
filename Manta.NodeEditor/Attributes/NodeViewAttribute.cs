using Avalonia.Controls;

namespace Manta.NodeEditor.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public abstract class NodeViewAttribute : Attribute
{
    public abstract Control CreateView();
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class NodeViewAttribute<T> : NodeViewAttribute where T : Control, new()
{
    public override Control CreateView() => new T();
}
