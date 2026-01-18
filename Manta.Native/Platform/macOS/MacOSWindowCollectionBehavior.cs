using System.Diagnostics.CodeAnalysis;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     macOS window collection behavior flags.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
[Flags]
public enum MacOSWindowCollectionBehavior : uint
{
    Default = 0,
    CanJoinAllSpaces = 1 << 0,
    MoveToActiveSpace = 1 << 1,
    Managed = 1 << 2,
    Transient = 1 << 3,
    Stationary = 1 << 4,
    ParticipatesInCycle = 1 << 5,
    IgnoresCycle = 1 << 6,
    FullScreenPrimary = 1 << 7,
    FullScreenAuxiliary = 1 << 8,
    FullScreenNone = 1 << 9,
    FullScreenAllowsTiling = 1 << 11,
    FullScreenDisallowsTiling = 1 << 12
}
