namespace Manta.Media.Imaging;

sealed class LocalImageSource(string localPath) : IImageSource
{
    public string LocalPath { get; } = localPath;
}
