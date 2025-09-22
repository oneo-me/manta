using Avalonia;

namespace Axe.Themes.Values;

public sealed class ThemeRefValue : AvaloniaObject
{
    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<ThemeRefValue, object?>(
        nameof(Value));

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}
