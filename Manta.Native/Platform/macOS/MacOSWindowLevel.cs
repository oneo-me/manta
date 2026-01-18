using System.Diagnostics.CodeAnalysis;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     macOS window level constants.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MacOSWindowLevel
{
    Normal = 0,
    Floating = 3,
    Submenu = 4,
    TornOffMenu = 5,
    MainMenu = 11,
    Status = 12,
    ModalPanel = 13,
    PopUpMenu = 101,
    ScreenSaver = 1000
}
