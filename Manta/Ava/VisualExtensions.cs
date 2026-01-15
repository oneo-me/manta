using System.Runtime.CompilerServices;
using Avalonia;

namespace Manta.Ava;

public static class VisualExtensions
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_VisualRoot")]
    static extern Visual? VisualRoot(Visual? visual);

    public static Visual? GetVisualRoot(this Visual visual)
    {
        return VisualRoot(visual)?.Parent as Visual;
    }
}
