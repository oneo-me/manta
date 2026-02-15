using System.Text.Json.Serialization;

namespace Manta.Courier.Protocol;

public sealed class Envelope
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public required EnvelopeType Type { get; init; }

    [JsonPropertyName("method")]
    public string? Method { get; init; }

    [JsonPropertyName("payload")]
    public string? Payload { get; init; }
}
