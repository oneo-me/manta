using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
static partial class MacOSWindow
{
    const string LibraryName = "libMantaNative";

    [LibraryImport(LibraryName, EntryPoint = "manta_window_configure_window")]
    internal static partial void ConfigureWindow(IntPtr windowPtr);

    [LibraryImport(LibraryName, EntryPoint = "manta_window_configure_tray_window")]
    internal static partial void ConfigureTrayWindow(IntPtr windowPtr);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static void OnSheetCompleted(nint state)
    {
        var handle = GCHandle.FromIntPtr(state);
        var tcs = (TaskCompletionSource)handle.Target!;
        handle.Free();
        tcs.SetResult();
    }

    [LibraryImport(LibraryName, EntryPoint = "manta_window_begin_sheet")]
    private static unsafe partial void BeginSheet(IntPtr ownerPtr, IntPtr windowPtr, delegate* unmanaged[Cdecl]<nint, void> callback, nint state);

    public static unsafe Task ShowSheetAsync(IntPtr ownerPtr, IntPtr windowPtr)
    {
        var tcs = new TaskCompletionSource();
        var handle = GCHandle.Alloc(tcs);
        BeginSheet(ownerPtr, windowPtr, &OnSheetCompleted, GCHandle.ToIntPtr(handle));
        return tcs.Task;
    }
}
