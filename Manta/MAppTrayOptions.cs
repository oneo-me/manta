using Manta.Reactive.Views;

namespace Manta;

public sealed class MAppTrayOptions
{
    public required Func<TrayWindowModel> Model { get; init; }
    public required byte[] Icon { get; init; }
}
