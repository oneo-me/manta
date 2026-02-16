using Avalonia.Media;

namespace Manta.NodeEditor.TypeSystem;

/// <summary>
/// Maps port value types to colors for visual type distinction.
/// </summary>
public static class PortColors
{
    static readonly Dictionary<Type, Color> _colors = new()
    {
        [typeof(double)] = Color.Parse("#4A9EFF"),
        [typeof(float)] = Color.Parse("#7EC8FF"),
        [typeof(int)] = Color.Parse("#2DD4BF"),
        [typeof(string)] = Color.Parse("#4ADE80"),
        [typeof(bool)] = Color.Parse("#FB923C"),
    };

    static readonly Color _fallback = Color.Parse("#A0A0A0");

    static readonly Dictionary<Type, IBrush> _brushCache = new();

    public static Color GetColor(Type type)
        => _colors.GetValueOrDefault(type, _fallback);

    public static IBrush GetBrush(Type type)
    {
        if (_brushCache.TryGetValue(type, out var brush))
            return brush;

        brush = new SolidColorBrush(GetColor(type)).ToImmutable();
        _brushCache[type] = brush;
        return brush;
    }
}
