using System.Text;

namespace AirKit.Reactive.System;

public static class FileInfoExtension
{
    static readonly UTF8Encoding _utf8NoBom = new(false);

    public static string GetFileNameWithoutExtension(this FileInfo fileInfo)
    {
        return Path.GetFileNameWithoutExtension(fileInfo.FullName);
    }

    public static string ReadText(this FileInfo fileInfo, Encoding? encoding = null)
    {
        using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream, encoding ?? _utf8NoBom);
        return streamReader.ReadToEnd();
    }

    public static async Task<string> ReadTextAsync(this FileInfo fileInfo, Encoding? encoding = null)
    {
        var fileStream = fileInfo.OpenRead();
        var streamReader = new StreamReader(fileStream, encoding ?? _utf8NoBom);
        var result = await streamReader.ReadToEndAsync().ConfigureAwait(false);
        streamReader.Dispose();
        await fileStream.DisposeAsync().ConfigureAwait(false);
        return result;
    }

    public static void WriteText(this FileInfo fileInfo, string text, Encoding? encoding = null)
    {
        using var fileStream = fileInfo.OpenWrite();
        using var streamWriter = new StreamWriter(fileStream, encoding ?? _utf8NoBom);
        streamWriter.Write(text);
    }

    public static async Task WriteTextAsync(this FileInfo fileInfo, string text, Encoding? encoding = null)
    {
        var fileStream = fileInfo.OpenWrite();
        var streamWriter = new StreamWriter(fileStream, encoding ?? _utf8NoBom);
        await streamWriter.WriteAsync(text).ConfigureAwait(false);
        await streamWriter.DisposeAsync().ConfigureAwait(false);
        await fileStream.DisposeAsync().ConfigureAwait(false);
    }
}
