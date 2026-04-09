using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
static partial class MacOSWindow
{
    const string LibraryName = "libMantaNative";

    [LibraryImport(LibraryName, EntryPoint = "manta_window_configure_window")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    internal static partial void ConfigureWindow(IntPtr windowPtr);
}
