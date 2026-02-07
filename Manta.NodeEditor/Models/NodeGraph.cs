using System.Collections.ObjectModel;
using Manta.Reactive.Views;

namespace Manta.NodeEditor.Models;

public sealed class NodeGraph : Model
{
    public ObservableCollection<NodeBase> Nodes { get; } = [];

    public ObservableCollection<NodeConnection> Connections { get; } = [];

    public void AddNode(NodeBase node)
    {
        Nodes.Add(node);
    }

    public void RemoveNode(NodeBase node)
    {
        var connectionsToRemove = Connections
            .Where(c => c.SourceNode == node || c.TargetNode == node)
            .ToList();

        foreach (var conn in connectionsToRemove)
        {
            conn.Target.IsConnected = false;
            conn.Source.IsConnected = Connections.Any(c => c != conn && c.Source == conn.Source);
            Connections.Remove(conn);
        }

        Nodes.Remove(node);
    }

    public NodeConnection? Connect(NodePort source, NodePort target)
    {
        if (source.Direction != PortDirection.Output || target.Direction != PortDirection.Input)
            return null;

        if (source.Owner == target.Owner)
            return null;

        // Input port can only have one connection
        var existing = Connections.FirstOrDefault(c => c.Target == target);
        if (existing is not null)
            Disconnect(existing);

        var connection = new NodeConnection
        {
            Source = source,
            Target = target
        };

        Connections.Add(connection);
        source.IsConnected = true;
        target.IsConnected = true;

        return connection;
    }

    public void Disconnect(NodeConnection connection)
    {
        Connections.Remove(connection);
        connection.Target.IsConnected = false;
        connection.Source.IsConnected = Connections.Any(c => c.Source == connection.Source);
    }

    public void ClearSelection()
    {
        foreach (var node in Nodes)
            node.IsSelected = false;
        foreach (var conn in Connections)
            conn.IsSelected = false;
    }

    public void DeleteSelected()
    {
        var selectedConnections = Connections.Where(c => c.IsSelected).ToList();
        foreach (var conn in selectedConnections)
            Disconnect(conn);

        var selectedNodes = Nodes.Where(n => n.IsSelected).ToList();
        foreach (var node in selectedNodes)
            RemoveNode(node);
    }

    public IEnumerable<NodeConnection> GetConnectionsForPort(NodePort port)
    {
        return Connections.Where(c => c.Source == port || c.Target == port);
    }
}
