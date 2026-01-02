using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Manta.Media.Imaging;

namespace Manta.Controls;

public class MAvatar : ContentControl
{
    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<MAvatar, double>(
        nameof(Size));

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<double> LoadingSizeProperty = AvaloniaProperty.Register<MAvatar, double>(
        nameof(LoadingSize));

    public double LoadingSize
    {
        get => GetValue(LoadingSizeProperty);
        set => SetValue(LoadingSizeProperty, value);
    }

    public static readonly StyledProperty<IImageSource?> SourceProperty = AvaloniaProperty.Register<MAvatar, IImageSource?>(
        nameof(Source));

    public IImageSource? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<BitmapBlendingMode> BlendModeProperty = AvaloniaProperty.Register<MAvatar, BitmapBlendingMode>(
        nameof(BlendMode));

    public BitmapBlendingMode BlendMode
    {
        get => GetValue(BlendModeProperty);
        set => SetValue(BlendModeProperty, value);
    }

    public static readonly StyledProperty<Stretch> StretchProperty = AvaloniaProperty.Register<MAvatar, Stretch>(
        nameof(Stretch), Stretch.Uniform);

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public static readonly StyledProperty<StretchDirection> StretchDirectionProperty = AvaloniaProperty.Register<MAvatar, StretchDirection>(
        nameof(StretchDirection), StretchDirection.Both);

    public StretchDirection StretchDirection
    {
        get => GetValue(StretchDirectionProperty);
        set => SetValue(StretchDirectionProperty, value);
    }
}
