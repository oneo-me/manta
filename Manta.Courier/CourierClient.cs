using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Channels;
using Manta.Courier.Protocol;

namespace Manta.Courier;

public sealed class CourierClient(string pipeName) : IAsyncDisposable
{
    NamedPipeClientStream? _pipeStream;
    Task? _readLoop;
    CancellationTokenSource? _cts;

    readonly ConcurrentDictionary<string, TaskCompletionSource<Envelope>> _pendingRequests = new();
    readonly ConcurrentDictionary<string, Channel<Envelope>> _pendingStreams = new();

    public async Task ConnectAsync(CancellationToken ct = default)
    {
        _cts = new();
        _pipeStream = new(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        await _pipeStream.ConnectAsync(ct).ConfigureAwait(false);
        _readLoop = Task.Run(() => ReadLoopAsync(_cts.Token), _cts.Token);
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>
    (
        string method, TRequest request,
        CancellationToken ct = default
    )
    {
        var id = Guid.NewGuid().ToString("N");
        var tcs = new TaskCompletionSource<Envelope>(TaskCreationOptions.RunContinuationsAsynchronously);
        _pendingRequests[id] = tcs;

        await using var reg = ct.Register(() => tcs.TrySetCanceled(ct));

        try
        {
            var envelope = new Envelope
            {
                Id = id,
                Type = EnvelopeType.Request,
                Method = method,
                Payload = JsonSerializer.Serialize(request)
            };
            await PipeProtocol.WriteAsync(_pipeStream!, envelope, ct).ConfigureAwait(false);

            var response = await tcs.Task.ConfigureAwait(false);

            if (response.Type == EnvelopeType.Error)
                throw new CourierException(response.Payload ?? "Unknown error");

            return JsonSerializer.Deserialize<TResponse>(response.Payload!)!;
        }
        finally
        {
            _pendingRequests.TryRemove(id, out _);
        }
    }

    public async IAsyncEnumerable<TResponse> StreamAsync<TRequest, TResponse>
    (
        string method, TRequest request,
        [EnumeratorCancellation] CancellationToken ct = default
    )
    {
        var id = Guid.NewGuid().ToString("N");
        var channel = Channel.CreateUnbounded<Envelope>(new()
        {
            SingleReader = true,
            SingleWriter = true
        });
        _pendingStreams[id] = channel;

        try
        {
            var envelope = new Envelope
            {
                Id = id,
                Type = EnvelopeType.Request,
                Method = method,
                Payload = JsonSerializer.Serialize(request)
            };
            await PipeProtocol.WriteAsync(_pipeStream!, envelope, ct).ConfigureAwait(false);

            await foreach (var item in channel.Reader.ReadAllAsync(ct).ConfigureAwait(false))
            {
                if (item.Type == EnvelopeType.Error)
                    throw new CourierException(item.Payload ?? "Unknown error");

                if (item.Type == EnvelopeType.StreamEnd)
                    yield break;

                yield return JsonSerializer.Deserialize<TResponse>(item.Payload!)!;
            }
        }
        finally
        {
            _pendingStreams.TryRemove(id, out _);
        }
    }

    async Task ReadLoopAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested && _pipeStream is { IsConnected: true })
            {
                var envelope = await PipeProtocol.ReadAsync(_pipeStream, ct).ConfigureAwait(false);
                if (envelope is null)
                    break;

                DispatchResponse(envelope);
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
            CompleteAllPending();
        }
    }

    void DispatchResponse(Envelope envelope)
    {
        switch (envelope.Type)
        {
            case EnvelopeType.Response:
            case EnvelopeType.Error when _pendingRequests.ContainsKey(envelope.Id):
                if (_pendingRequests.TryRemove(envelope.Id, out var tcs))
                    tcs.TrySetResult(envelope);
                break;

            case EnvelopeType.StreamItem:
            case EnvelopeType.StreamEnd:
            case EnvelopeType.Error:
                if (_pendingStreams.TryGetValue(envelope.Id, out var channel))
                {
                    channel.Writer.TryWrite(envelope);
                    if (envelope.Type is EnvelopeType.StreamEnd or EnvelopeType.Error)
                        channel.Writer.TryComplete();
                }

                break;
        }
    }

    void CompleteAllPending()
    {
        foreach (var (_, tcs) in _pendingRequests)
            tcs.TrySetException(new CourierException("Connection closed"));
        _pendingRequests.Clear();

        foreach (var (_, channel) in _pendingStreams)
            channel.Writer.TryComplete(new CourierException("Connection closed"));
        _pendingStreams.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync().ConfigureAwait(false);
            _cts.Dispose();
            _cts = null;
        }

        if (_readLoop is not null)
        {
            try
            {
                await _readLoop.ConfigureAwait(false);
            }
            catch
            {
                /* swallow */
            }

            _readLoop = null;
        }

        if (_pipeStream is not null)
        {
            await _pipeStream.DisposeAsync().ConfigureAwait(false);
            _pipeStream = null;
        }
    }
}
