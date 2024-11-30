using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AirKit.Controls;

public class LightScrollViewer : ScrollViewer
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<LightScrollViewer, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
