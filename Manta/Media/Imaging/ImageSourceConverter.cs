using System.ComponentModel;
using System.Globalization;
using Avalonia.Markup.Xaml;

namespace Manta.Media.Imaging;

sealed class ImageSourceConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var sourcew = (string)value;
        var uri = sourcew.StartsWith('/') ? new(sourcew, UriKind.Relative) : new Uri(sourcew, UriKind.RelativeOrAbsolute);

        if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            return new WebImageSource(uri);

        if (uri is { IsAbsoluteUri: true, IsFile: true })
            return new LocalImageSource(uri.LocalPath);

        if (context?.GetService(typeof(IUriContext)) is IUriContext uriContext)
            return new AvaloniaImageSource(uri, uriContext.BaseUri);

        return null;
    }
}
