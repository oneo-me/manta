using Manta.Controls.Icons;

namespace Manta.DevTools;

static class Icons
{
    public static IconSource ViewModelIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Sticker-Square--Streamline-Solar-Ar.svg"));
    public static IconSource WindowModelIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Window-Frame--Streamline-Solar-Ar.svg"));
    public static IconSource RegionModelIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Widget--Streamline-Solar-Ar.svg"));
    public static IconSource DialogModelIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Slider-Horizontal--Streamline-Solar-Ar.svg"));
}
