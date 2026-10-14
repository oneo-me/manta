using Avalonia.Media.Imaging;
using Manta.Controls.Icons;

namespace MantaDemo;

static class Icons
{
    public static Bitmap AppIcon { get; } = App.Assets.GetBitmap("AppIcon.png");

    public static IconSource Home { get; } = App.Assets.GetIcon("Icons/Home-Smile-Angle--Streamline-Solar-Ar.svg");
    public static IconSource Styles { get; } = App.Assets.GetIcon("Icons/Cat--Streamline-Solar-Ar.svg");
    public static IconSource Reactive { get; } = App.Assets.GetIcon("Icons/Fire--Streamline-Solar-Ar.svg");
    public static IconSource Controls { get; } = App.Assets.GetIcon("Icons/Code-Circle--Streamline-Solar-Ar.svg");
    public static IconSource MultiLanguage { get; } = App.Assets.GetIcon("Icons/Planet--Streamline-Solar-Ar.svg");

    public static IconSource Copy { get; } = App.Assets.GetIcon("Icons/Copy--Streamline-Solar-Ar.svg");
    public static IconSource Export { get; } = App.Assets.GetIcon("Icons/Upload--Streamline-Solar-Ar.svg");

    public static IconSource ThemeSystem { get; } = App.Assets.GetIcon("Icons/Sun-Fog--Streamline-Solar-Ar.svg");
    public static IconSource ThemeLight { get; } = App.Assets.GetIcon("Icons/Sun--Streamline-Solar-Ar.svg");
    public static IconSource ThemeDark { get; } = App.Assets.GetIcon("Icons/Moon-Stars--Streamline-Solar-Ar.svg");
    public static IconSource Settings { get; } = App.Assets.GetIcon("Icons/Settings--Streamline-Solar-Ar.svg");
    public static IconSource TweakcnTool { get; } = App.Assets.GetIcon("Icons/Ruler-Pen--Streamline-Solar-Ar.svg");

    public static IconSource Color { get; } = App.Assets.GetIcon("Icons/Palette-Round--Streamline-Solar-Ar.svg");
    public static IconSource Text { get; } = App.Assets.GetIcon("Icons/Text-Circle--Streamline-Solar-Ar.svg");
    public static IconSource Radius { get; } = App.Assets.GetIcon("Icons/Structure--Streamline-Solar-Ar.svg");
    public static IconSource Ring { get; } = App.Assets.GetIcon("Icons/Stop--Streamline-Solar-Ar.svg");
    public static IconSource Shadow { get; } = App.Assets.GetIcon("Icons/Sticker-Circle--Streamline-Solar-Ar.svg");
    public static IconSource Spacing { get; } = App.Assets.GetIcon("Icons/Posts-Carousel-Horizontal--Streamline-Solar-Ar.svg");
    public static IconSource Thickness { get; } = App.Assets.GetIcon("Icons/Minimize-Square-2--Streamline-Solar-Ar.svg");
}
