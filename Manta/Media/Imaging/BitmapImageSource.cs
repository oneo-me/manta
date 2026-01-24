using Avalonia.Media.Imaging;

namespace Manta.Media.Imaging;

sealed class BitmapImageSource(Bitmap bitmap) : IImageSource
{
    public Bitmap Bitmap { get; } = bitmap;
}
