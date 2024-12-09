using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;

namespace AirKit.Themes;

sealed class SystemAccentColors : ResourceProvider
{
    static IPlatformSettings? GetFromOwner(IResourceHost? owner)
    {
        return owner switch
        {
            Application app => app.PlatformSettings,
            Visual visual   => TopLevel.GetTopLevel(visual)?.PlatformSettings,
            _               => null
        };
    }

    IPlatformSettings? _platformSettings;

    protected override void OnAddOwner(IResourceHost owner)
    {
        base.OnAddOwner(owner);

        if (_platformSettings is not null)
            return;

        _platformSettings = GetFromOwner(Owner);
        if (_platformSettings is null)
            return;

        ReacquireColors(_platformSettings.GetColorValues());
        _platformSettings.ColorValuesChanged += PlatformSettingsOnColorValuesChanged;
    }

    void PlatformSettingsOnColorValuesChanged(object? sender, PlatformColorValues e)
    {
        ReacquireColors(e);

        // need to raise twice to ensure that the colors are updated
        RaiseResourcesChanged();
        RaiseResourcesChanged();
    }

    static Color DefaultAccentColor { get; } = Color.FromRgb(0, 122, 255);
    public static Color? AppSystemAccentColor { get; internal set; }

    Color _systemAccentColor;

    void ReacquireColors(PlatformColorValues? colorValues)
    {
        _systemAccentColor = AppSystemAccentColor ?? colorValues?.AccentColor1 ?? DefaultAccentColor;
    }

    const string AccentKey = "SystemAccentColor";

    public override bool HasResources => true;

    public override bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (key is string strKey)
            value = strKey switch
            {
                AccentKey => _systemAccentColor,
                _         => null
            };
        else
            value = null;

        return value is not null;
    }
}
