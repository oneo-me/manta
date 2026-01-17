using Manta.Reactive.Views;

namespace Manta;

public sealed class MAppTaryOptions
{
    public required TrayWindowModel Model { get; init; }
    public required byte[] Icon { get; init; }
}
