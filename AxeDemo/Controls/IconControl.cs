using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Axe.Reactive;
using Path = Avalonia.Controls.Shapes.Path;

namespace AxeDemo.Controls;

public sealed class IconControl : TemplatedControl
{
    public static readonly StyledProperty<IconKey> KeyProperty = AvaloniaProperty.Register<IconControl, IconKey>(
        nameof(Key));

    public IconKey Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    Viewbox? PART_ViewBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_ViewBox = e.NameScope.Get<Viewbox>(nameof(PART_ViewBox));
        OnIconChanged(Key);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == KeyProperty)
            OnIconChanged((IconKey)change.NewValue!);
    }

    void OnIconChanged(IconKey key)
    {
        var viewBox = PART_ViewBox;
        if (viewBox is null)
            return;

        var panel = new Panel();
        viewBox.Child = panel;

        var xml = Assets.GetString($"Icons/{key switch {
            IconKey.Overview      => "Home-Smile-Angle--Streamline-Solar-Ar",
            IconKey.Styles        => "Cat--Streamline-Solar-Ar",
            IconKey.Reactive      => "Fire--Streamline-Solar-Ar",
            IconKey.MultiLanguage => "Planet--Streamline-Solar-Ar",
            IconKey.Controls      => "Code-Circle--Streamline-Solar-Ar",
            IconKey.Settings      => "Settings--Streamline-Solar-Ar",
            IconKey.ThemeLight    => "Sun--Streamline-Solar-Ar",
            IconKey.ThemeDark     => "Moon-Stars--Streamline-Solar-Ar",
            IconKey.ThemeSystem   => "Sun-Fog--Streamline-Solar-Ar",
            _                     => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        }}.svg");

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var root = xmlDoc.DocumentElement!;
        panel.Width = double.Parse(root.Attributes["width"]!.Value);
        panel.Height = double.Parse(root.Attributes["height"]!.Value);

        foreach (XmlElement node in root.ChildNodes)
            if (node.Name == "path")
            {
                var path = new Path
                {
                    [!Shape.FillProperty] = this[!BackgroundProperty],
                    Data = Geometry.Parse(node.Attributes["d"]!.Value)
                };

                if (node.Attributes["stroke"] != null)
                {
                    path[!Shape.StrokeProperty] = this[!ForegroundProperty];
                    path.StrokeLineCap = node.Attributes["stroke-linecap"]?.Value switch
                    {
                        "round"  => PenLineCap.Round,
                        "square" => PenLineCap.Square,
                        _        => PenLineCap.Flat
                    };
                    path.StrokeThickness = double.Parse(node.Attributes["stroke-width"]!.Value);
                }

                panel.Children.Add(path);
            }
    }
}
