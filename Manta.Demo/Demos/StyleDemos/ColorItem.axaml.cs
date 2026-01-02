using Avalonia;
using Avalonia.Media;
using Manta.Controls;

namespace Manta.Demo.Demos.StyleDemos;

sealed partial class ColorItem : MView
{
    public ColorItem()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string> KeyProperty = AvaloniaProperty.Register<ColorItem, string>(
        nameof(Key));

    public string Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public static readonly StyledProperty<IBrush?> BrushProperty = AvaloniaProperty.Register<ColorItem, IBrush?>(
        nameof(Brush));

    public IBrush? Brush
    {
        get => GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }
}
