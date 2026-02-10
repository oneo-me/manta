using Avalonia.Media;

namespace Manta.NodeEditor.TypeSystem;

/// <summary>
/// Maps port value types to colors for visual type distinction.
/// </summary>
public static class PortColors
{
    static readonly Dictionary<Type, Color> Colors = new()
    {
        [typeof(double)] = Color.Parse("#4A9EFF"),
        [typeof(float)] = Color.Parse("#7EC8FF"),
        [typeof(int)] = Color.Parse("#2DD4BF"),
        [typeof(string)] = Color.Parse("#4ADE80"),
        [typeof(bool)] = Color.Parse("#FB923C"),
    };

    static readonly Color Fallback = Color.Parse("#A0A0A0");

    static readonly Dictionary<Type, IBrush> BrushCache = new();

    public static Color GetColor(Type type)
        => Colors.GetValueOrDefault(type, Fallback);

    public static IBrush GetBrush(Type type)
    {
        if (BrushCache.TryGetValue(type, out var brush))
            return brush;

        brush = new SolidColorBrush(GetColor(type)).ToImmutable();
        BrushCache[type] = brush;
        return brush;
    }
}
