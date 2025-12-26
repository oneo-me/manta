using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace LightKit.Animation;

public sealed class SmoothBrushTransition : InterpolatingTransitionBase<IBrush?>
{
    protected override IBrush? Interpolate(double progress, IBrush? from, IBrush? to)
    {
        if (from is null) return to;
        if (to is null) return from;

        if (from is ISolidColorBrush fromSolid && to is ISolidColorBrush toSolid)
        {
            var fromColor = ApplyBrushOpacity(fromSolid.Color, fromSolid.Opacity);
            var toColor = ApplyBrushOpacity(toSolid.Color, toSolid.Opacity);
            var color = InterpolateColorPremultiplied(fromColor, toColor, progress);
            return new ImmutableSolidColorBrush(color);
        }

        if (from is IGradientBrush fromGradient && to is IGradientBrush toGradient)
        {
            var fromColor = fromGradient.GradientStops.Count > 0
                ? fromGradient.GradientStops[0].Color
                : Colors.Transparent;
            var toColor = toGradient.GradientStops.Count > 0
                ? toGradient.GradientStops[0].Color
                : Colors.Transparent;

            fromColor = ApplyBrushOpacity(fromColor, fromGradient.Opacity);
            toColor = ApplyBrushOpacity(toColor, toGradient.Opacity);

            var color = InterpolateColorPremultiplied(fromColor, toColor, progress);
            return new ImmutableSolidColorBrush(color);
        }

        if (from is ISolidColorBrush fromSolidMixed)
        {
            var fromColor = ApplyBrushOpacity(fromSolidMixed.Color, fromSolidMixed.Opacity);
            var toColor = GetRepresentativeColor(to);
            var color = InterpolateColorPremultiplied(fromColor, toColor, progress);
            return new ImmutableSolidColorBrush(color);
        }

        if (to is ISolidColorBrush toSolidMixed)
        {
            var fromColor = GetRepresentativeColor(from);
            var toColor = ApplyBrushOpacity(toSolidMixed.Color, toSolidMixed.Opacity);
            var color = InterpolateColorPremultiplied(fromColor, toColor, progress);
            return new ImmutableSolidColorBrush(color);
        }

        return progress < 0.5 ? from : to;
    }

    static Color GetRepresentativeColor(IBrush brush)
    {
        return brush switch
        {
            ISolidColorBrush solid => ApplyBrushOpacity(solid.Color, solid.Opacity),
            IGradientBrush gradient when gradient.GradientStops.Count > 0 =>
                ApplyBrushOpacity(gradient.GradientStops[0].Color, gradient.Opacity),
            _ => Colors.Transparent
        };
    }

    static Color ApplyBrushOpacity(Color color, double opacity)
    {
        var alpha = (byte)Math.Round(color.A * opacity);
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }

    static Color InterpolateColorPremultiplied(Color from, Color to, double t)
    {
        var fromA = from.A / 255.0;
        var toA = to.A / 255.0;

        var fromR = from.R * fromA;
        var fromG = from.G * fromA;
        var fromB = from.B * fromA;

        var toR = to.R * toA;
        var toG = to.G * toA;
        var toB = to.B * toA;

        var resA = fromA + (toA - fromA) * t;
        var resR = fromR + (toR - fromR) * t;
        var resG = fromG + (toG - fromG) * t;
        var resB = fromB + (toB - fromB) * t;

        if (resA < 0.001)
            return Color.FromArgb(0, 0, 0, 0);

        var a = (byte)Math.Round(resA * 255);
        var r = (byte)Math.Round(Math.Clamp(resR / resA, 0, 255));
        var g = (byte)Math.Round(Math.Clamp(resG / resA, 0, 255));
        var b = (byte)Math.Round(Math.Clamp(resB / resA, 0, 255));

        return Color.FromArgb(a, r, g, b);
    }
}
