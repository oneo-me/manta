using Avalonia;

namespace Manta.NodeEditor.Models;

public sealed class Graph
{
    public List<Node> Nodes { get; } = [];
    public List<Connection> Connections { get; } = [];

    public Node? SelectedNode => Nodes.Find(n => n.IsSelected);
    public Connection? SelectedConnection => Connections.Find(c => c.IsSelected);

    public Node AddNode(NodeTypeInfo typeInfo, Point position)
    {
        var instance = (NodeBase)Activator.CreateInstance(typeInfo.NodeType)!;
        instance.Position = position;

        var nodeId = Guid.NewGuid();
        instance.Id = nodeId;

        var inputPorts = typeInfo.InputPortDefinitions.Select(def => new Port
        {
            Id = Guid.NewGuid(),
            Definition = def,
            Owner = null! // Will be set below
        }).ToList();

        var outputPorts = typeInfo.OutputPortDefinitions.Select(def => new Port
        {
            Id = Guid.NewGuid(),
            Definition = def,
            Owner = null! // Will be set below
        }).ToList();

        var node = new Node
        {
            Id = nodeId,
            Instance = instance,
            TypeInfo = typeInfo,
            InputPorts = inputPorts,
            OutputPorts = outputPorts
        };

        // Set owner reference
        foreach (var port in inputPorts)
        {
            typeof(Port).GetProperty(nameof(Port.Owner))!.SetValue(port, node);
            instance.InputPorts.Add(port);
        }

        foreach (var port in outputPorts)
        {
            typeof(Port).GetProperty(nameof(Port.Owner))!.SetValue(port, node);
            instance.OutputPorts.Add(port);
        }

        Nodes.Add(node);
        return node;
    }

    public Connection? AddConnection(Port sourcePort, Port targetPort)
    {
        if (sourcePort.IsInput || targetPort.IsOutput)
            return null;

        // Remove existing connection to input port (input can only have one connection)
        var existingConnection = Connections.Find(c => c.TargetPort == targetPort);
        if (existingConnection != null)
            Connections.Remove(existingConnection);

        var connection = new Connection
        {
            Id = Guid.NewGuid(),
            SourcePort = sourcePort,
            TargetPort = targetPort
        };

        Connections.Add(connection);
        return connection;
    }

    public void RemoveNode(Node node)
    {
        // Remove all connections to/from this node
        Connections.RemoveAll(c => c.SourceNode == node || c.TargetNode == node);
        Nodes.Remove(node);
    }

    public void RemoveConnection(Connection connection)
    {
        Connections.Remove(connection);
    }

    public void ClearSelection()
    {
        foreach (var node in Nodes)
            node.IsSelected = false;
        foreach (var connection in Connections)
            connection.IsSelected = false;
    }

    public void SelectNode(Node node)
    {
        ClearSelection();
        node.IsSelected = true;
    }

    public void SelectConnection(Connection connection)
    {
        ClearSelection();
        connection.IsSelected = true;
    }

    public IEnumerable<Connection> GetConnectionsForPort(Port port)
    {
        return Connections.Where(c => c.SourcePort == port || c.TargetPort == port);
    }
}
