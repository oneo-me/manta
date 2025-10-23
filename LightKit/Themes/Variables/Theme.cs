using Avalonia;

namespace LightKit.Themes.Variables;

public enum ColorScheme
{
    Normal,
    Primary,
    Success,
    Warning,
    Danger
}

public class Theme
{
    public static readonly AttachedProperty<ColorScheme> ColorSchemeProperty =
        AvaloniaProperty.RegisterAttached<Theme, AvaloniaObject, ColorScheme>("ColorScheme", ColorScheme.Normal, true);

    public static void SetColorScheme(AvaloniaObject obj, ColorScheme value)
    {
        obj.SetValue(ColorSchemeProperty, value);
    }

    public static ColorScheme GetColorScheme(AvaloniaObject obj)
    {
        return obj.GetValue(ColorSchemeProperty);
    }
}
