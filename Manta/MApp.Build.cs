using Avalonia;
using Avalonia.Media;
using Manta.Logging.Inner;

namespace Manta;

#pragma warning disable CA1001
public partial class MApp
#pragma warning restore CA1001
{
    public static AppBuilder Build<T>() where T : MApp, new()
    {
        return AppBuilder.Configure<T>()
            .UsePlatformDetect()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = OperatingSystem.IsMacOS() ? "PingFang SC, Helvetica Neue, Helvetica, Arial, sans-serif" :
                    OperatingSystem.IsWindows() ? "Microsoft YaHei UI, Segoe UI, Arial, sans-serif" :
                    OperatingSystem.IsLinux() ? "Noto Sans CJK SC" :
                    throw new PlatformNotSupportedException()
            })
            .LogToAxe();
    }
}
