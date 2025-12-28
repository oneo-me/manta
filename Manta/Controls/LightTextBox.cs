using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

public class LightTextBox : TextBox
{
    public static readonly StyledProperty<bool> IsPasswordBoxProperty = AvaloniaProperty.Register<LightTextBox, bool>(
        nameof(IsPasswordBox));

    public bool IsPasswordBox
    {
        get => GetValue(IsPasswordBoxProperty);
        set => SetValue(IsPasswordBoxProperty, value);
    }

    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<LightTextBox, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
