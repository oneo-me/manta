using Avalonia.Interactivity;
using Manta.MindMap.Models;

namespace Manta.MindMap.Controls;

public sealed class EdgeSelectedEventArgs(RoutedEvent routedEvent, MindMapEdge edge) : RoutedEventArgs(routedEvent)
{
    public MindMapEdge Edge { get; } = edge;
}
