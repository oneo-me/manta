using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Manta.Courier.Protocol;

namespace Manta.Courier;

public sealed class CourierServer(string pipeName) : IDisposable, IAsyncDisposable
{
    readonly Dictionary<string, Func<string?, CancellationToken, Task<object?>>> _unaryHandlers = new();
    readonly Dictionary<string, Func<string?, CancellationToken, IAsyncEnumerable<object?>>> _streamHandlers = new();
    CancellationTokenSource? _cts;

    public void On<TRequest>(string method, Func<TRequest, CancellationToken, Task> handler)
    {
        On<TRequest, object?>(method, async (request, ct) =>
        {
            await handler(request, ct).ConfigureAwait(false);
            return null;
        });
    }

    public void On<TRequest, TResponse>(string method, Func<TRequest, CancellationToken, Task<TResponse>> handler)
    {
        _unaryHandlers[method] = async (payload, ct) =>
        {
            var request = Deserialize<TRequest>(payload);
            var response = await handler(request, ct).ConfigureAwait(false);
            return response;
        };
    }

    public void OnStream<TRequest, TResponse>
    (
        string method,
        Func<TRequest, CancellationToken, IAsyncEnumerable<TResponse>> handler
    )
    {
        _streamHandlers[method] = (payload, ct) =>
        {
            var request = Deserialize<TRequest>(payload);
            return WrapAsyncEnumerable(handler(request, ct), CancellationToken.None);
        };
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var token = _cts.Token;

        while (!token.IsCancellationRequested)
        {
            var serverStream = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

            try
            {
                await serverStream.WaitForConnectionAsync(token).ConfigureAwait(false);
                _ = HandleClientAsync(serverStream, token);
            }
            catch (OperationCanceledException)
            {
                await serverStream.DisposeAsync().ConfigureAwait(false);
                break;
            }
            catch
            {
                await serverStream.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    async Task HandleClientAsync(NamedPipeServerStream stream, CancellationToken ct)
    {
        try
        {
            while (stream.IsConnected && !ct.IsCancellationRequested)
            {
                var envelope = await PipeProtocol.ReadAsync(stream, ct).ConfigureAwait(false);
                if (envelope is null)
                    break;

                if (envelope.Type != EnvelopeType.Request)
                    continue;

                await ProcessRequestAsync(stream, envelope, ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (IOException)
        {
        }
        finally
        {
            await stream.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task ProcessRequestAsync(Stream stream, Envelope request, CancellationToken ct)
    {
        var method = request.Method ?? string.Empty;

        if (_streamHandlers.TryGetValue(method, out var streamHandler))
            await ProcessStreamRequestAsync(stream, request.Id, streamHandler, request.Payload, ct)
                .ConfigureAwait(false);
        else if (_unaryHandlers.TryGetValue(method, out var unaryHandler))
            await ProcessUnaryRequestAsync(stream, request.Id, unaryHandler, request.Payload, ct)
                .ConfigureAwait(false);
        else
            await SendErrorAsync(stream, request.Id, $"Unknown method: {method}", ct).ConfigureAwait(false);
    }

    static async Task ProcessUnaryRequestAsync
    (
        Stream stream, string id,
        Func<string?, CancellationToken, Task<object?>> handler, string? payload, CancellationToken ct
    )
    {
        try
        {
            var result = await handler(payload, ct).ConfigureAwait(false);
            var response = new Envelope
            {
                Id = id,
                Type = EnvelopeType.Response,
                Payload = JsonSerializer.Serialize(result)
            };
            await PipeProtocol.WriteAsync(stream, response, ct).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            await SendErrorAsync(stream, id, ex.Message, ct).ConfigureAwait(false);
        }
    }

    static async Task ProcessStreamRequestAsync
    (
        Stream stream, string id,
        Func<string?, CancellationToken, IAsyncEnumerable<object?>> handler, string? payload, CancellationToken ct
    )
    {
        try
        {
            await foreach (var item in handler(payload, ct).WithCancellation(ct).ConfigureAwait(false))
            {
                var streamItem = new Envelope
                {
                    Id = id,
                    Type = EnvelopeType.StreamItem,
                    Payload = JsonSerializer.Serialize(item)
                };
                await PipeProtocol.WriteAsync(stream, streamItem, ct).ConfigureAwait(false);
            }

            var streamEnd = new Envelope
            {
                Id = id,
                Type = EnvelopeType.StreamEnd
            };
            await PipeProtocol.WriteAsync(stream, streamEnd, ct).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            await SendErrorAsync(stream, id, ex.Message, ct).ConfigureAwait(false);
        }
    }

    static async Task SendErrorAsync(Stream stream, string id, string message, CancellationToken ct)
    {
        var error = new Envelope
        {
            Id = id,
            Type = EnvelopeType.Error,
            Payload = message
        };
        await PipeProtocol.WriteAsync(stream, error, ct).ConfigureAwait(false);
    }

    static T Deserialize<T>(string? payload)
    {
        if (payload is null)
            return default!;
        return JsonSerializer.Deserialize<T>(payload)!;
    }

    static async IAsyncEnumerable<object?> WrapAsyncEnumerable<T>
    (
        IAsyncEnumerable<T> source,
        [EnumeratorCancellation] CancellationToken ct = default
    )
    {
        await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            yield return item;
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync().ConfigureAwait(false);
            _cts.Dispose();
            _cts = null;
        }
    }

    public void Dispose()
    {
        if (_cts is not null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
