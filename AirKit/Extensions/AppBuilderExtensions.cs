using AirKit.Logging;
using Avalonia;
using Avalonia.Media;

namespace AirKit.Extensions;

public static class AppBuilderExtensions
{
    public static AppBuilder UseFontFamily(this AppBuilder builder, string fontFamily)
    {
        Log.Verbose("App", "Use font family: {fontFamily}", fontFamily);

        return builder.With(new FontManagerOptions
        {
            DefaultFamilyName = fontFamily,
            FontFallbacks =
            [
                new()
                {
                    FontFamily = new(fontFamily)
                }
            ]
        });
    }

    public static AppBuilder UseDefaultFontFamily(this AppBuilder builder)
    {
        return builder.UseFontFamily(OperatingSystem.IsMacOS() ? "PingFang SC" : OperatingSystem.IsWindows() ? "Microsoft YaHei UI" : string.Empty);
    }
}
