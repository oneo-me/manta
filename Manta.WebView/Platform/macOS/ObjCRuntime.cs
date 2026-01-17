using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Manta.WebView.Platform.macOS;

/// <summary>
///     Objective-C runtime interop for macOS native calls
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
static class ObjCRuntime
{
    const string LibObjC = "/usr/lib/libobjc.dylib";

    [DllImport(LibObjC, EntryPoint = "objc_getClass")]
    public static extern IntPtr GetClass(string name);

    [DllImport(LibObjC, EntryPoint = "sel_registerName")]
    public static extern IntPtr GetSelector(string name);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern IntPtr SendMessage(IntPtr receiver, IntPtr selector);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern IntPtr SendMessage(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern IntPtr SendMessage(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern IntPtr SendMessage(IntPtr receiver, IntPtr selector, double arg1, double arg2, double arg3, double arg4);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendVoid(IntPtr receiver, IntPtr selector);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendVoid(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendVoid(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendVoid(IntPtr receiver, IntPtr selector, CGRect frame);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendVoid(IntPtr receiver, IntPtr selector, double arg1, double arg2, double arg3, double arg4);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern bool SendBool(IntPtr receiver, IntPtr selector);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern void SendDouble(IntPtr receiver, IntPtr selector, double arg1);

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    public static extern double GetDouble(IntPtr receiver, IntPtr selector);

    /// <summary>
    ///     Release an Objective-C object
    /// </summary>
    public static void Release(IntPtr obj)
    {
        if (obj != IntPtr.Zero)
        {
            var release = GetSelector("release");
            SendVoid(obj, release);
        }
    }

    /// <summary>
    ///     Autorelease an Objective-C object
    /// </summary>
    public static IntPtr Autorelease(IntPtr obj)
    {
        if (obj == IntPtr.Zero)
            return IntPtr.Zero;

        var autorelease = GetSelector("autorelease");
        return SendMessage(obj, autorelease);
    }

    // Check if an object is a kind of class
    public static bool IsKindOfClass(IntPtr obj, string className)
    {
        if (obj == IntPtr.Zero)
            return false;

        var classObj = GetClass(className);
        var isKindOf = GetSelector("isKindOfClass:");
        return SendBool(obj, isKindOf, classObj);
    }

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    static extern bool SendBool(IntPtr receiver, IntPtr selector, IntPtr arg1);

    /// <summary>
    ///     Create an NSString from a C# string. Caller is responsible for releasing the returned object.
    /// </summary>
    public static IntPtr CreateNSString(string str)
    {
        if (string.IsNullOrEmpty(str))
            return IntPtr.Zero;

        var nsStringClass = GetClass("NSString");
        var alloc = GetSelector("alloc");
        var nsString = SendMessage(nsStringClass, alloc);

        var initWithUTF8String = GetSelector("initWithUTF8String:");
        return SendMessageWithCString(nsString, initWithUTF8String, str);
    }

    [DllImport(LibObjC, EntryPoint = "objc_msgSend")]
    static extern IntPtr SendMessageWithCString(IntPtr receiver, IntPtr selector, string str);

    /// <summary>
    ///     Create an NSURL from a string. Caller is responsible for releasing the returned object.
    /// </summary>
    public static IntPtr CreateNSURL(string urlString)
    {
        if (string.IsNullOrEmpty(urlString))
            return IntPtr.Zero;

        var nsUrlClass = GetClass("NSURL");
        var urlWithString = GetSelector("URLWithString:");
        var nsString = CreateNSString(urlString);

        if (nsString == IntPtr.Zero)
            return IntPtr.Zero;

        var url = SendMessage(nsUrlClass, urlWithString, nsString);

        // Release the temporary NSString
        Release(nsString);

        // The URL is autoreleased, so retain it for the caller
        if (url != IntPtr.Zero)
        {
            var retain = GetSelector("retain");
            SendMessage(url, retain);
        }

        return url;
    }

    // CGRect structure for frame initialization
    [StructLayout(LayoutKind.Sequential)]
    public struct CGRect(double x, double y, double width, double height)
    {
        public double X = x;
        public double Y = y;
        public double Width = width;
        public double Height = height;
    }
}
