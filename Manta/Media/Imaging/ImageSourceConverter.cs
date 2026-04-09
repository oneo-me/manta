using System.ComponentModel;
using System.Globalization;
using Avalonia.Markup.Xaml;
using Manta.Controls;

namespace Manta.Media.Imaging;

public sealed class ImageSourceConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        Uri? baseUri = null;

        if (context?.GetService(typeof(IUriContext)) is IUriContext uriContext)
            baseUri = uriContext.BaseUri;

        return MImage.ConvertToImageSource((string)value, baseUri);
    }
}
