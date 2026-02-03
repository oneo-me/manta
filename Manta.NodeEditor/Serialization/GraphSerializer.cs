using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Manta.NodeEditor.Attributes;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Serialization;

public sealed class GraphSerializer(NodeRegistry registry)
{
    static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public const int CurrentVersion = 1;

    public string Serialize(Graph graph)
    {
        var data = new GraphData
        {
            Version = CurrentVersion,
            Nodes = graph.Nodes.Select(SerializeNode).ToList(),
            Connections = graph.Connections.Select(SerializeConnection).ToList()
        };

        return JsonSerializer.Serialize(data, _jsonOptions);
    }

    public async Task SaveAsync(Graph graph, string filePath)
    {
        var json = Serialize(graph);
        await File.WriteAllTextAsync(filePath, json);
    }

    public Graph Deserialize(string json)
    {
        var data = JsonSerializer.Deserialize<GraphData>(json, _jsonOptions)
                   ?? throw new InvalidOperationException("Failed to deserialize graph data");

        if (data.Version > CurrentVersion)
            throw new InvalidOperationException(
                $"Graph file version {data.Version} is not supported. Maximum supported version is {CurrentVersion}.");

        var graph = new Graph();
        var nodeMap = new Dictionary<Guid, Node>();

        // Create nodes
        foreach (var nodeData in data.Nodes)
        {
            var typeInfo = registry.GetNodeTypeInfo(nodeData.TypeFullName)
                           ?? throw new InvalidOperationException($"Node type '{nodeData.TypeFullName}' not found in registry");

            var node = graph.AddNode(typeInfo, new(nodeData.X, nodeData.Y));

            // Restore node ID
            typeof(Node).GetProperty(nameof(Node.Id))!.SetValue(node, nodeData.Id);
            node.Instance.Id = nodeData.Id;

            // Restore properties
            RestoreProperties(node.Instance, nodeData.Properties);

            nodeMap[nodeData.Id] = node;
        }

        // Create connections
        foreach (var connData in data.Connections)
        {
            if (!nodeMap.TryGetValue(connData.SourceNodeId, out var sourceNode) ||
                !nodeMap.TryGetValue(connData.TargetNodeId, out var targetNode))
                continue;

            var sourcePort = sourceNode.OutputPorts.Find(p => p.Name == connData.SourcePortName);
            var targetPort = targetNode.InputPorts.Find(p => p.Name == connData.TargetPortName);

            if (sourcePort != null && targetPort != null)
            {
                var connection = graph.AddConnection(sourcePort, targetPort);
                if (connection != null)
                    typeof(Connection).GetProperty(nameof(Connection.Id))!.SetValue(connection, connData.Id);
            }
        }

        return graph;
    }

    public async Task<Graph> LoadAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        return Deserialize(json);
    }

    NodeData SerializeNode(Node node)
    {
        var properties = new Dictionary<string, object?>();
        var type = node.Instance.GetType();

        // Get non-port properties
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetCustomAttribute<InputAttribute>() != null ||
                prop.GetCustomAttribute<OutputAttribute>() != null)
                continue;

            if (!prop.CanRead || !prop.CanWrite)
                continue;

            if (prop.Name is nameof(NodeBase.Id) or nameof(NodeBase.Position) or nameof(NodeBase.IsSelected))
                continue;

            var value = prop.GetValue(node.Instance);
            if (value != null)
                properties[prop.Name] = value;
        }

        return new()
        {
            Id = node.Id,
            TypeFullName = node.TypeInfo.TypeFullName,
            X = node.Position.X,
            Y = node.Position.Y,
            Properties = properties
        };
    }

    static ConnectionData SerializeConnection(Connection connection)
    {
        return new()
        {
            Id = connection.Id,
            SourceNodeId = connection.SourceNode.Id,
            SourcePortName = connection.SourcePort.Name,
            TargetNodeId = connection.TargetNode.Id,
            TargetPortName = connection.TargetPort.Name
        };
    }

    static void RestoreProperties(NodeBase instance, Dictionary<string, object?> properties)
    {
        var type = instance.GetType();

        foreach (var (name, value) in properties)
        {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (prop is { CanWrite: true } && value != null)
                try
                {
                    var convertedValue = ConvertJsonElement(value, prop.PropertyType);
                    prop.SetValue(instance, convertedValue);
                }
                catch
                {
                    // Ignore property restoration failures
                }
        }
    }

    static object? ConvertJsonElement(object? value, Type targetType)
    {
        if (value is JsonElement element)
            return element.ValueKind switch
            {
                JsonValueKind.String                                    => element.GetString(),
                JsonValueKind.Number when targetType == typeof(int)     => element.GetInt32(),
                JsonValueKind.Number when targetType == typeof(long)    => element.GetInt64(),
                JsonValueKind.Number when targetType == typeof(float)   => element.GetSingle(),
                JsonValueKind.Number when targetType == typeof(double)  => element.GetDouble(),
                JsonValueKind.Number when targetType == typeof(decimal) => element.GetDecimal(),
                JsonValueKind.True                                      => true,
                JsonValueKind.False                                     => false,
                JsonValueKind.Null                                      => null,
                _                                                       => value
            };

        return value;
    }
}
