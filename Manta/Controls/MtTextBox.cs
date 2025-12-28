using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

public class MtTextBox : TextBox
{
    public static readonly StyledProperty<bool> IsPasswordBoxProperty = AvaloniaProperty.Register<MtTextBox, bool>(
        nameof(IsPasswordBox));

    public bool IsPasswordBox
    {
        get => GetValue(IsPasswordBoxProperty);
        set => SetValue(IsPasswordBoxProperty, value);
    }

    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MtTextBox, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
