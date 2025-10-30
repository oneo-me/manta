using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace LightKit.Controls;

public enum LightTextBoxStyle
{
    None,

    Outlined
}

public class LightTextBox : TextBox
{
    public static readonly StyledProperty<LightTextBoxStyle> CustomProperty = AvaloniaProperty.Register<LightTextBox, LightTextBoxStyle>(
        nameof(Custom));

    public LightTextBoxStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }

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
