using System.ComponentModel;

namespace Manta.Media.Imaging;

[TypeConverter(typeof(ImageSourceConverter))]
public interface IImageSource;
