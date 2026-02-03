using System.Text.Json.Serialization;

namespace Manta.NodeEditor.Serialization;

public sealed class GraphData
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    [JsonPropertyName("nodes")]
    public List<NodeData> Nodes { get; set; } = [];

    [JsonPropertyName("connections")]
    public List<ConnectionData> Connections { get; set; } = [];
}

public sealed class NodeData
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string TypeFullName { get; set; } = "";

    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, object?> Properties { get; set; } = new();
}

public sealed class ConnectionData
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("sourceNodeId")]
    public Guid SourceNodeId { get; set; }

    [JsonPropertyName("sourcePortName")]
    public string SourcePortName { get; set; } = "";

    [JsonPropertyName("targetNodeId")]
    public Guid TargetNodeId { get; set; }

    [JsonPropertyName("targetPortName")]
    public string TargetPortName { get; set; } = "";
}
