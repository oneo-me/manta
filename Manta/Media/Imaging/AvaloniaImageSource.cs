namespace Manta.Media.Imaging;

sealed class AvaloniaImageSource(Uri uri, Uri? baseUri) : IImageSource
{
    public Uri Uri { get; } = uri;
    public Uri? BaseUri { get; } = baseUri;
}
