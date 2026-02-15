using System.Buffers.Binary;
using System.Text;
using System.Text.Json;

namespace Manta.Courier.Protocol;

public static class PipeProtocol
{
    static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task WriteAsync(Stream stream, Envelope envelope, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(envelope, _jsonOptions);
        var payload = Encoding.UTF8.GetBytes(json);

        var lengthBytes = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(lengthBytes, payload.Length);

        await stream.WriteAsync(lengthBytes, ct).ConfigureAwait(false);
        await stream.WriteAsync(payload, ct).ConfigureAwait(false);
        await stream.FlushAsync(ct).ConfigureAwait(false);
    }

    public static async Task<Envelope?> ReadAsync(Stream stream, CancellationToken ct = default)
    {
        var lengthBytes = new byte[4];
        var bytesRead = await ReadExactAsync(stream, lengthBytes, ct).ConfigureAwait(false);
        if (bytesRead == 0)
            return null;

        var length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);
        if (length is <= 0 or > 64 * 1024 * 1024)
            throw new InvalidOperationException($"Invalid message length: {length}");

        var payload = new byte[length];
        bytesRead = await ReadExactAsync(stream, payload, ct).ConfigureAwait(false);
        if (bytesRead == 0)
            return null;

        return JsonSerializer.Deserialize<Envelope>(payload, _jsonOptions);
    }

    static async Task<int> ReadExactAsync(Stream stream, byte[] buffer, CancellationToken ct)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset, buffer.Length - offset), ct)
                .ConfigureAwait(false);
            if (read == 0)
                return 0;
            offset += read;
        }

        return offset;
    }
}
