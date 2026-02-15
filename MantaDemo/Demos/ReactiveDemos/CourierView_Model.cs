using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Avalonia.Threading;
using Manta.Courier;
using Manta.Reactive.Commands;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<CourierView>]
sealed class CourierView_Model : DemoViewModel
{
    public static CourierView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Courier;

    const string PipeName = "manta-courier-demo";

    CourierServer? _server;
    CourierClient? _client;
    CancellationTokenSource? _serverCts;

    // --- Server ---

    public bool IsServerRunning
    {
        get;
        private set => SetValue(ref field, value);
    }

    public ObservableCollection<string> ServerLogs { get; } = [];

    public CommandWarper StartServerCommand => GetCommand(async () =>
    {
        if (IsServerRunning) return;

        _server = new(PipeName);

        _server.On<EchoRequest, EchoResponse>("Echo", (req, _) =>
        {
            var message = $"Echo: {req.Message}";
            AddServerLog($"[Echo] Received: \"{req.Message}\" → Responding: \"{message}\"");
            return Task.FromResult(new EchoResponse { Message = message });
        });

        _server.OnStream<CountdownRequest, CountdownItem>("Countdown", (req, ct) => CountdownStream(req.From, ct));

        _serverCts = new();
        IsServerRunning = true;
        AddServerLog("Server started.");

        try
        {
            await _server.RunAsync(_serverCts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            IsServerRunning = false;
            AddServerLog("Server stopped.");
        }
    });

    async IAsyncEnumerable<CountdownItem> CountdownStream
    (
        int from,
        [EnumeratorCancellation] CancellationToken ct
    )
    {
        AddServerLog($"[Countdown] Starting from {from}");
        for (var i = from; i >= 0; i--)
        {
            ct.ThrowIfCancellationRequested();
            AddServerLog($"[Countdown] Yielding {i}");
            yield return new() { Value = i };
            if (i > 0)
                await Task.Delay(500, ct);
        }

        AddServerLog("[Countdown] Stream completed.");
    }

    public CommandWarper StopServerCommand => GetCommand(async () =>
    {
        if (!IsServerRunning || _serverCts is null) return;
        await _serverCts.CancelAsync();
        if (_server is not null)
        {
            await _server.DisposeAsync();
            _server = null;
        }

        _serverCts.Dispose();
        _serverCts = null;
    });

    public CommandWarper ClearServerLogsCommand => GetCommand(() => ServerLogs.Clear());

    void AddServerLog(string message)
    {
        Dispatcher.UIThread.Post(() => ServerLogs.Add($"[{DateTime.Now:HH:mm:ss}] {message}"));
    }

    // --- Client ---

    public bool IsClientConnected
    {
        get;
        private set => SetValue(ref field, value);
    }

    public string EchoInput
    {
        get;
        set => SetValue(ref field, value);
    } = "Hello Courier!";

    public int CountdownFrom
    {
        get;
        set => SetValue(ref field, value);
    } = 5;

    public ObservableCollection<string> ClientLogs { get; } = [];

    public CommandWarper ConnectCommand => GetCommand(async () =>
    {
        if (IsClientConnected) return;

        _client = new(PipeName);
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await _client.ConnectAsync(cts.Token);
            IsClientConnected = true;
            AddClientLog("Connected to server.");
        }
        catch (Exception ex)
        {
            AddClientLog($"Connect failed: {ex.Message}");
            await _client.DisposeAsync();
            _client = null;
        }
    });

    public CommandWarper DisconnectCommand => GetCommand(async () =>
    {
        if (!IsClientConnected || _client is null) return;
        await _client.DisposeAsync();
        _client = null;
        IsClientConnected = false;
        AddClientLog("Disconnected.");
    });

    public CommandWarper SendEchoCommand => GetCommand(async () =>
    {
        if (_client is null) return;
        try
        {
            AddClientLog($"[Echo] Sending: \"{EchoInput}\"");
            var response = await _client.RequestAsync<EchoRequest, EchoResponse>("Echo",
                new() { Message = EchoInput });
            AddClientLog($"[Echo] Response: \"{response.Message}\"");
        }
        catch (Exception ex)
        {
            AddClientLog($"[Echo] Error: {ex.Message}");
        }
    });

    public CommandWarper StartCountdownCommand => GetCommand(async () =>
    {
        if (_client is null) return;
        try
        {
            AddClientLog($"[Countdown] Requesting countdown from {CountdownFrom}...");
            await foreach (var item in _client.StreamAsync<CountdownRequest, CountdownItem>("Countdown",
                               new() { From = CountdownFrom }))
                AddClientLog($"[Countdown] Received: {item.Value}");
            AddClientLog("[Countdown] Stream ended.");
        }
        catch (Exception ex)
        {
            AddClientLog($"[Countdown] Error: {ex.Message}");
        }
    });

    public CommandWarper ClearClientLogsCommand => GetCommand(() => ClientLogs.Clear());

    void AddClientLog(string message)
    {
        Dispatcher.UIThread.Post(() => ClientLogs.Add($"[{DateTime.Now:HH:mm:ss}] {message}"));
    }

    // --- Cleanup ---

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        Disposable.Create(() => _ = CleanupAsync()).DisposeWith(disposables);
    }

    async Task CleanupAsync()
    {
        if (_client is not null)
        {
            await _client.DisposeAsync();
            _client = null;
            IsClientConnected = false;
        }

        if (_serverCts is not null)
        {
            await _serverCts.CancelAsync();
            _serverCts.Dispose();
            _serverCts = null;
        }

        if (_server is not null)
        {
            await _server.DisposeAsync();
            _server = null;
        }
    }
}

// --- DTOs ---

sealed class EchoRequest
{
    public string Message { get; init; } = string.Empty;
}

sealed class EchoResponse
{
    public string Message { get; init; } = string.Empty;
}

sealed class CountdownRequest
{
    public int From { get; init; }
}

sealed class CountdownItem
{
    public int Value { get; init; }
}
