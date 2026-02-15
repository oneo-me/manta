using Avalonia.Interactivity;
using Manta.MindMap.Models;

namespace Manta.MindMap.Controls;

public sealed class NodeSelectedEventArgs(RoutedEvent routedEvent, MindMapNode node) : RoutedEventArgs(routedEvent)
{
    public MindMapNode Node { get; } = node;
}
