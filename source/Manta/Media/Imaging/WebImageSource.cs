namespace Manta.Media.Imaging;

sealed class WebImageSource(Uri uri) : IImageSource
{
    public Uri Uri { get; } = uri;
}
