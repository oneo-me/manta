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
        if (OperatingSystem.IsMacOS())
            return builder.UseFontFamily("PingFang SC");
        if (OperatingSystem.IsWindows())
            return builder.UseFontFamily("Microsoft YaHei UI");
        if (OperatingSystem.IsLinux())
            return builder.UseFontFamily("Noto Sans CJK SC");

        return builder;
    }
}
