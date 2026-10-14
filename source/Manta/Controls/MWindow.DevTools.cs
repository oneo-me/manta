using Avalonia.Controls;

namespace Manta.Controls;

public sealed partial class MWindow
{
    internal static Action<MWindow, Grid>? PrepareAttach { get; set; }
}
