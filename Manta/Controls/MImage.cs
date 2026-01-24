using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Manta.Logging;
using Manta.Media.Imaging;
using Manta.Reactive.Services;
using Manta.Services;

namespace Manta.Controls;

[TemplatePart(nameof(PART_Image), typeof(Image))]
public class MImage : ContentControl
{
    public static readonly StyledProperty<double> LoadingSizeProperty = AvaloniaProperty.Register<MImage, double>(
        nameof(LoadingSize));

    public double LoadingSize
    {
        get => GetValue(LoadingSizeProperty);
        set => SetValue(LoadingSizeProperty, value);
    }

    public static readonly StyledProperty<IImageSource?> SourceProperty = AvaloniaProperty.Register<MImage, IImageSource?>(
        nameof(Source));

    public IImageSource? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<BitmapBlendingMode> BlendModeProperty = AvaloniaProperty.Register<MImage, BitmapBlendingMode>(
        nameof(BlendMode));

    public BitmapBlendingMode BlendMode
    {
        get => GetValue(BlendModeProperty);
        set => SetValue(BlendModeProperty, value);
    }

    public static readonly StyledProperty<Stretch> StretchProperty = AvaloniaProperty.Register<MImage, Stretch>(
        nameof(Stretch), Stretch.Uniform);

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public static readonly StyledProperty<StretchDirection> StretchDirectionProperty = AvaloniaProperty.Register<MImage, StretchDirection>(
        nameof(StretchDirection), StretchDirection.Both);

    public StretchDirection StretchDirection
    {
        get => GetValue(StretchDirectionProperty);
        set => SetValue(StretchDirectionProperty, value);
    }

    Image? PART_Image;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Image = e.NameScope.Get<Image>(nameof(PART_Image));

        UpdateImage(Source);
    }

    public void SetImageSource(IImageSource? source)
    {
        SetValue(SourceProperty, source);
    }

    internal static IImageSource ConvertToImageSource(string source, Uri? baseUri)
    {
        var uri = source.StartsWith('/') ? new(source, UriKind.Relative) : new Uri(source, UriKind.RelativeOrAbsolute);

        if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            return new WebImageSource(uri);

        if (uri is { IsAbsoluteUri: true, IsFile: true })
            return new LocalImageSource(uri.LocalPath);

        return new AvaloniaImageSource(uri, baseUri);
    }

    public void SetImageSource(string? source)
    {
        if (source == null)
        {
            Source = null;
            return;
        }

        Source = ConvertToImageSource(source, null);
    }

    public void SetImageSource(Bitmap? source)
    {
        if (source == null)
        {
            Source = null;
            return;
        }

        Source = new BitmapImageSource(source);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty)
            UpdateImage(change.GetNewValue<IImageSource?>());
    }

    CancellationTokenSource? _updateCancellationToken;

    async void UpdateImage(IImageSource? source)
    {
        try
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var oldCancellationToken = Interlocked.Exchange(ref _updateCancellationToken, cancellationTokenSource);
            try
            {
                if (oldCancellationToken is not null)
                    await oldCancellationToken.CancelAsync();
            }
            catch
            {
                // ignored
            }

            if (PART_Image == null)
                return;

            if (source is null)
            {
                PART_Image.Source = null;

                PseudoClasses.Set(":loading", false);
                PseudoClasses.Set(":image", false);
                PseudoClasses.Set(":no-image", true);
                return;
            }

            PseudoClasses.Set(":loading", true);

            var image = await Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(10, cancellationTokenSource.Token);

                    var imageLoadService = Service.GetService<ImageLoadService>();
                    return await imageLoadService.LoadAsync(source, cancellationTokenSource.Token);
                }
                catch (Exception e)
                {
                    Log.Warning("Failed to load image: {0}", e);
                    return null;
                }
                finally
                {
                    cancellationTokenSource.Dispose();
                }
            }, CancellationToken.None);

            PART_Image.Source = image;

            PseudoClasses.Set(":loading", false);
            PseudoClasses.Set(":image", image != null);
            PseudoClasses.Set(":no-image", image == null);
        }
        catch (Exception e)
        {
            Log.Warning("Failed to update image: {0}", e);
        }
    }
}
