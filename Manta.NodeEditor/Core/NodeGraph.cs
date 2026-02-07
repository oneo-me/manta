using System.Collections.ObjectModel;
using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// Contains nodes and connections. The central data model for the node editor.
/// Uses ObservableCollection for automatic UI synchronization.
/// </summary>
public sealed class NodeGraph : Model
{
    public ObservableCollection<NodeBase> Nodes { get; } = [];

    public ObservableCollection<Connection> Connections { get; } = [];

    /// <summary>
    /// Add a node to the graph at the specified position.
    /// </summary>
    public T Add<T>(double x = 0, double y = 0) where T : NodeBase, new()
    {
        var node = new T { X = x, Y = y };
        // Touch Inputs/Output to trigger port discovery
        _ = node.Inputs;
        Nodes.Add(node);
        return node;
    }

    /// <summary>
    /// Add an existing node instance to the graph.
    /// </summary>
    public void Add(NodeBase node)
    {
        _ = node.Inputs;
        Nodes.Add(node);
    }

    /// <summary>
    /// Remove a node and all its connections from the graph.
    /// </summary>
    public void Remove(NodeBase node)
    {
        var toRemove = Connections
            .Where(c => c.Source.Node == node || c.Target.Node == node)
            .ToList();

        foreach (var conn in toRemove)
            Disconnect(conn);

        Nodes.Remove(node);
    }

    /// <summary>
    /// Connect an output port to an input port.
    /// Each input port can only have one incoming connection.
    /// </summary>
    public Connection? Connect(Port source, Port target)
    {
        if (!source.IsOutput || !target.IsInput)
            return null;

        if (source.Node == target.Node)
            return null;

        // Remove existing connection to this input
        var existing = Connections.FirstOrDefault(c => c.Target == target);
        if (existing is not null)
            Disconnect(existing);

        var connection = new Connection(source, target);
        Connections.Add(connection);
        source.IsConnected = true;
        target.IsConnected = true;

        return connection;
    }

    /// <summary>
    /// Remove a connection and update port states.
    /// </summary>
    public void Disconnect(Connection connection)
    {
        Connections.Remove(connection);
        connection.Target.IsConnected = false;
        connection.Source.IsConnected = Connections.Any(c => c.Source == connection.Source);
    }

    /// <summary>
    /// Clear all selection states.
    /// </summary>
    public void ClearSelection()
    {
        foreach (var node in Nodes)
            node.IsSelected = false;
        foreach (var conn in Connections)
            conn.IsSelected = false;
    }

    /// <summary>
    /// Delete all selected nodes and connections.
    /// </summary>
    public void DeleteSelected()
    {
        var selectedConns = Connections.Where(c => c.IsSelected).ToList();
        foreach (var conn in selectedConns)
            Disconnect(conn);

        var selectedNodes = Nodes.Where(n => n.IsSelected).ToList();
        foreach (var node in selectedNodes)
            Remove(node);
    }
}
