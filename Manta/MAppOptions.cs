using Manta.Reactive.Views;

namespace Manta;

public sealed class MAppOptions
{
    public Func<WindowModel>? MainModel { get; init; }
    public MAppTrayOptions? Tray { get; init; }
    public Action? Exit { get; init; }
}
