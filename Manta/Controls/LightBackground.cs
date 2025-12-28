using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Manta.Controls;

public class LightBackground : ContentControl
{
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
    public override void Render(DrawingContext context)
    {
        context.Custom(new BlurBehindRenderOperation(new(default, Bounds.Size)));
    }

    sealed class BlurBehindRenderOperation(Rect bounds) : ICustomDrawOperation
    {
        public void Dispose()
        {
        }

        public bool HitTest(Point p)
        {
            return bounds.Contains(p);
        }

        public Rect Bounds => bounds;

        public bool Equals(ICustomDrawOperation? other)
        {
            return other is BlurBehindRenderOperation op && op.Bounds == Bounds;
        }

        public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();

            using var lease = leaseFeature?.Lease();
            if (lease?.SkSurface is null)
                return;

            var canvas = lease.SkCanvas;
            if (canvas.TotalMatrix.TryInvert(out var currentInvertedTransform) is false)
                return;

            using var backgroundSnapshot = lease.SkSurface.Snapshot();
            using var backdropShader = SKShader.CreateImage(backgroundSnapshot, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, currentInvertedTransform);
            using var blurFilter = SKImageFilter.CreateBlur(15, 15, SKShaderTileMode.Clamp);
            using var blurPaint = new SKPaint();
            blurPaint.Shader = backdropShader;
            blurPaint.ImageFilter = blurFilter;

            canvas.DrawRect(0, 0, (float)Bounds.Width, (float)Bounds.Height, blurPaint);
        }
    }
}
