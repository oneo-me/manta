using Manta.Controls.Icons;

namespace Manta.DevTools;

static class Icons
{
    public static IconSource ViewModelIcon { get; } = Assets.GetIcon("Icons/Sticker-Square--Streamline-Solar-Ar.svg");
    public static IconSource WindowModelIcon { get; } = Assets.GetIcon("Icons/Window-Frame--Streamline-Solar-Ar.svg");
    public static IconSource RegionModelIcon { get; } = Assets.GetIcon("Icons/Widget--Streamline-Solar-Ar.svg");
    public static IconSource DialogModelIcon { get; } = Assets.GetIcon("Icons/Slider-Horizontal--Streamline-Solar-Ar.svg");

    public static IconSource Info { get; } = Assets.GetIcon("Icons/Info-Circle--Streamline-Solar-Ar.svg");
    public static IconSource Play { get; } = Assets.GetIcon("Icons/Play--Streamline-Solar-Ar.svg");
}
