namespace Manta.Native.Platform;

abstract class MantaWebView : IDisposable
{
    public abstract IntPtr Handle { get; }

    ~MantaWebView()
    {
        Dispose(false);
    }

    public abstract void LoadUrl(string url);
    public abstract void LoadHtmlString(string html, string? baseUrl = null);

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
