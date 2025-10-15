using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class TitleBarButton : Button
{
    public static readonly StyledProperty<TitleBarButtonType> TypeProperty = AvaloniaProperty.Register<TitleBarButton, TitleBarButtonType>(
        nameof(Type));

    public TitleBarButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly StyledProperty<Thickness> InnerMarginProperty = AvaloniaProperty.Register<TitleBarButton, Thickness>(
        nameof(InnerMargin));

    public Thickness InnerMargin
    {
        get => GetValue(InnerMarginProperty);
        set => SetValue(InnerMarginProperty, value);
    }
}
