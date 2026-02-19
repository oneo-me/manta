using System.IO.Pipes;
using System.Text;

namespace Manta;

sealed class MAppSingleInstance : IDisposable
{
    readonly Mutex? _mutex;
    readonly CancellationTokenSource? _cts;
    readonly string _pipeName;

    public bool IsNew { get; }
    public event EventHandler<string[]>? ReOpen;

    public MAppSingleInstance(string instanceName)
    {
        _pipeName = instanceName;
        var mutex = new Mutex(true, $"Global\\{instanceName}", out var createdNew);

        if (createdNew)
        {
            _mutex = mutex;
            IsNew = true;
            _cts = new();
            Task.Run(() => ListenAsync(_cts.Token));
        }
        else
        {
            mutex.Dispose();
            NotifyAndExit(instanceName);
        }
    }

    // Runs synchronously on the UI thread — blocks briefly but exits before
    // Avalonia can show any window or dock icon bounce.
    static void NotifyAndExit(string pipeName)
    {
        try
        {
            using var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
            client.Connect(5000);
            WriteArgs(client, MApp.CommandLineArgs.ToArray());
        }
        catch
        {
            // ignored
        }

        Environment.Exit(0);
    }

    async Task ListenAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var server = new NamedPipeServerStream(
                _pipeName,
                PipeDirection.In,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

            try
            {
                await server.WaitForConnectionAsync(ct).ConfigureAwait(false);
                _ = HandleConnectionAsync(server, ct);
            }
            catch (OperationCanceledException)
            {
                await server.DisposeAsync().ConfigureAwait(false);
                break;
            }
            catch
            {
                await server.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    async Task HandleConnectionAsync(NamedPipeServerStream server, CancellationToken ct)
    {
        try
        {
            var args = await ReadArgsAsync(server, ct).ConfigureAwait(false);
            await UIThread.InvokeAsync(() => ReOpen?.Invoke(this, args));
        }
        catch
        {
            // ignored
        }
        finally
        {
            await server.DisposeAsync().ConfigureAwait(false);
        }
    }

    static async Task<string[]> ReadArgsAsync(Stream stream, CancellationToken ct)
    {
        var buf = new byte[4];
        await stream.ReadExactlyAsync(buf, ct).ConfigureAwait(false);
        var count = BitConverter.ToInt32(buf);

        var args = new string[count];
        for (var i = 0; i < count; i++)
        {
            await stream.ReadExactlyAsync(buf, ct).ConfigureAwait(false);
            var len = BitConverter.ToInt32(buf);
            var data = new byte[len];
            await stream.ReadExactlyAsync(data, ct).ConfigureAwait(false);
            args[i] = Encoding.UTF8.GetString(data);
        }

        return args;
    }

    static void WriteArgs(Stream stream, string[] args)
    {
        Span<byte> buf = stackalloc byte[4];
        BitConverter.TryWriteBytes(buf, args.Length);
        stream.Write(buf);
        foreach (var arg in args)
        {
            var data = Encoding.UTF8.GetBytes(arg);
            BitConverter.TryWriteBytes(buf, data.Length);
            stream.Write(buf);
            stream.Write(data);
        }

        stream.Flush();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _mutex?.Dispose();
    }
}
