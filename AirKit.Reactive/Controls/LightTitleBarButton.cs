using Avalonia;
using Avalonia.Controls;

namespace AirKit.Reactive.Controls;

public class LightTitleBarButton : Button
{
    public static readonly StyledProperty<LightTitleBarButtonType> TypeProperty = AvaloniaProperty.Register<LightTitleBarButton, LightTitleBarButtonType>(
        nameof(Type));

    public LightTitleBarButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly StyledProperty<Thickness> InnerMarginProperty = AvaloniaProperty.Register<LightTitleBarButton, Thickness>(
        nameof(InnerMargin));

    public Thickness InnerMargin
    {
        get => GetValue(InnerMarginProperty);
        set => SetValue(InnerMarginProperty, value);
    }
}
