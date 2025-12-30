using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Manta.Themes.Values;

namespace Manta.Charts.Renders;

/// <summary>
///     Shared context and utilities for chart rendering.
/// </summary>
sealed class ChartRenderContext
{
    public required FontFamily FontFamily { get; init; }
    public required IBrush? Foreground { get; init; }
    public required IBrush? Background { get; init; }
    public required Rect Bounds { get; init; }

    public static readonly Color[] DefaultColors =
    [
        new OklchColor { L = 0.7, C = 0.2 * 0.8, H = 134 }.ToColor(), // Green
        new OklchColor { L = 0.7, C = 0.25 * 0.8, H = 260 }.ToColor(), // Blue
        new OklchColor { L = 0.8, C = 0.3 * 0.8, H = 340 }.ToColor(), // Pink
        new OklchColor { L = 0.8, C = 0.3 * 0.8, H = 80 }.ToColor(), // Orange
        new OklchColor { L = 0.65, C = 0.25 * 0.8, H = 320 }.ToColor(), // Purple
        new OklchColor { L = 0.75, C = 0.25 * 0.8, H = 190 }.ToColor(), // Teal
        new OklchColor { L = 0.65, C = 0.3 * 0.8, H = 25 }.ToColor(), // Red
        new OklchColor { L = 0.9, C = 0.3 * 0.8, H = 90 }.ToColor() // Yellow
    ];

    public const double LabelFontSize = 12d;
    public const double LabelHeight = 18d;

    public Color BackgroundColor => (Background as ISolidColorBrush)?.Color ?? Colors.White;
    public Color ForegroundColor => (Foreground as ISolidColorBrush)?.Color ?? Colors.White;

    public FormattedText CreateText(string text, double fontSize, IBrush? brush, TextAlignment alignment = TextAlignment.Left)
    {
        return new(
            text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new(FontFamily),
            fontSize,
            brush ?? Brushes.White
        )
        {
            TextAlignment = alignment
        };
    }

    public static Color GetColor(int index, Color? customColor = null)
    {
        return customColor ?? DefaultColors[index % DefaultColors.Length];
    }

    public static Color BrightenColor(Color color, byte amount = 30)
    {
        return Color.FromArgb(255,
            (byte)Math.Min(255, color.R + amount),
            (byte)Math.Min(255, color.G + amount),
            (byte)Math.Min(255, color.B + amount));
    }
}
