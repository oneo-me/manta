using System.Collections.ObjectModel;
using Avalonia.Media;
using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;
using LightKit.Themes.Values;

namespace LightKit.Demo.Demos.StyleDemos;

[View<ColorView>]
sealed class ColorView_Model : DemoViewModel
{
    public static ColorView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Palette-Round--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Color;

    public ObservableCollection<SolidColorBrush> Colors { get; } = new();

    public double H
    {
        get;
        set
        {
            if (SetValue(ref field, value))
                OnColorChanged();
        }
    }

    public ColorView_Model()
    {
        OnColorChanged();
    }

    void OnColorChanged()
    {
        Colors.Clear();

        var themeColor = new ColorSet { H = H };

        var steps = 7;

        for (var i = steps - 1; i >= 0; i--)
            Colors.Add(new(themeColor.GetColor(i, steps).ToColor()));
    }
}
