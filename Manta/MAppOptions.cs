using Manta.Reactive.Views;

namespace Manta;

public sealed class MAppOptions
{
    public WindowModel? MainModel { get; init; }
    public MAppTaryOptions? Tary { get; init; }
    public Action? Exit { get; init; }
}
