using Avalonia;

namespace Manta.Themes.Variables;

public enum ColorScheme
{
    Normal,
    Primary,
    Success,
    Warning,
    Danger
}

public enum TextSize
{
    Normal,

    ExtraSmall,
    Small,

    Large,
    ExtraLarge,
    ExtraLarge2,
    ExtraLarge3,
    ExtraLarge4,
    ExtraLarge5,
    ExtraLarge6,
    ExtraLarge7,
    ExtraLarge8,
    ExtraLarge9
}

public class Mt
{
    public static readonly AttachedProperty<ColorScheme> ColorSchemeProperty =
        AvaloniaProperty.RegisterAttached<Mt, AvaloniaObject, ColorScheme>("ColorScheme", ColorScheme.Normal, true);

    public static void SetColorScheme(AvaloniaObject obj, ColorScheme value)
    {
        obj.SetValue(ColorSchemeProperty, value);
    }

    public static ColorScheme GetColorScheme(AvaloniaObject obj)
    {
        return obj.GetValue(ColorSchemeProperty);
    }

    public static readonly AttachedProperty<TextSize> TextSizeProperty =
        AvaloniaProperty.RegisterAttached<Mt, AvaloniaObject, TextSize>("TextSize", TextSize.Normal, true);

    public static void SetTextSize(AvaloniaObject obj, TextSize value)
    {
        obj.SetValue(TextSizeProperty, value);
    }

    public static TextSize GetTextSize(AvaloniaObject obj)
    {
        return obj.GetValue(TextSizeProperty);
    }
}
