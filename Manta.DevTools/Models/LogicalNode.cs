using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Manta.DevTools.Models;

sealed class LogicalNode : NodeBase
{
    public LogicalNode? Parent { get; }
    public ILogical Logical { get; }
    public LogicalContainer Container { get; }

    public LogicalNode(LogicalNode? parent, ILogical logical)
    {
        Parent = parent;
        Logical = logical;
        Container = new(this, logical);

        if (logical is StyledElement { Name.Length: > 0 } styledElement)
            Header = $"{logical.GetType().Name}: {styledElement.Name}";
        else
            Header = logical.GetType().Name;
    }

    public void UpdateAdorner(IResourceHost resourceHost)
    {
        if (Logical is Visual visual)
            UpdateAdornerCore(visual, resourceHost, true);
    }

    protected override void DisposeOverride()
    {
        Container.Dispose();
    }
}
