using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Utilities;

#pragma warning disable CS0618

namespace Manta.Controls;

public sealed partial class MWindow
{
    public void ProcessSheet(Window owner)
    {
        using (FreezeVisibilityChangeHandling(this))
        {
            EnsureStateBeforeShow(this);
            EnsureParentStateBeforeShow(this, owner);

            if (GetShown(this))
                throw new InvalidOperationException("The window is already being shown.");

            SetShowingAsDialog(this, true);
            RaiseEvent(new(WindowOpenedEvent));

            EnsureInitialized();
            ApplyStyling();
            SetShown(this, true);
            IsVisible = true;

            SetExpectedScaling(this, owner);

            var initialSize = new Size(
                double.IsNaN(Width) ? ClientSize.Width : Width,
                double.IsNaN(Height) ? ClientSize.Height : Height);

            var minMax = CreateMinMax(this);

            initialSize = new(
                MathUtilities.Clamp(initialSize.Width, minMax.MinWidth, minMax.MaxWidth),
                MathUtilities.Clamp(initialSize.Height, minMax.MinHeight, minMax.MaxHeight));

            var clientSizeChanged = initialSize != ClientSize;
            ClientSize = initialSize;

            ExecuteInitialLayoutPass(GetLayoutManager(this));

            if (SizeToContent.HasFlag(SizeToContent.Width))
            {
                initialSize = initialSize.WithWidth(MathUtilities.Clamp(GetArrangeBounds(this).Width, minMax.MinWidth, minMax.MaxWidth));
                clientSizeChanged |= initialSize != ClientSize;
                ClientSize = initialSize;
            }

            if (SizeToContent.HasFlag(SizeToContent.Height))
            {
                initialSize = initialSize.WithHeight(MathUtilities.Clamp(GetArrangeBounds(this).Height, minMax.MinHeight, minMax.MaxHeight));
                clientSizeChanged |= initialSize != ClientSize;
                ClientSize = initialSize;
            }

            Owner = owner;

            SetWindowStartupLocation(this, owner);

            SetDesktopScalingOverride(this, null);

            var platformImpl = GetPlatformImpl(this);

            if (clientSizeChanged || ClientSize != GetPlatformClientSize(platformImpl))
            {
                ResizePlatformImpl(platformImpl, ClientSize, WindowResizeReason.Layout);

                // we do not want PlatformImpl?.Resize to trigger HandleResized yet because it will set Width and Height.
                // So perform some important actions from HandleResized

                RendererResized(((IRenderRoot)this).Renderer, ClientSize);
                OnResized(CreateWindowResizedEventArgs(ClientSize, WindowResizeReason.Layout));

                if (!double.IsNaN(Width))
                    Width = ClientSize.Width;
                if (!double.IsNaN(Height))
                    Height = ClientSize.Height;
            }

            FrameSize = GetPlatformFrameSize(platformImpl);

            SetCanHandleResized(this, true);

            StartRendering();

            OnOpened(EventArgs.Empty);
        }
    }

    #region UnsafeAccessors for Window

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "FreezeVisibilityChangeHandling")]
    static extern IDisposable FreezeVisibilityChangeHandling(WindowBase window);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "EnsureStateBeforeShow")]
    static extern void EnsureStateBeforeShow(Window window);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "EnsureParentStateBeforeShow")]
    static extern void EnsureParentStateBeforeShow(Window window, Window owner);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_shown")]
    static extern ref bool GetShown(Window window);

    static void SetShown(Window window, bool value)
    {
        GetShown(window) = value;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_showingAsDialog")]
    static extern ref bool GetShowingAsDialog(Window window);

    static void SetShowingAsDialog(Window window, bool value)
    {
        GetShowingAsDialog(window) = value;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "SetExpectedScaling")]
    static extern void SetExpectedScaling(Window window, WindowBase? owner);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "SetWindowStartupLocation")]
    static extern void SetWindowStartupLocation(Window window, Window? owner);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_canHandleResized")]
    static extern ref bool GetCanHandleResized(Window window);

    static void SetCanHandleResized(Window window, bool value)
    {
        GetCanHandleResized(window) = value;
    }

    #endregion

    #region UnsafeAccessors for WindowBase

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_DesktopScalingOverride")]
    static extern void SetDesktopScalingOverride(WindowBase window, double? value);

    #endregion

    #region UnsafeAccessors for TopLevel

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PlatformImpl")]
    static extern IWindowImpl? GetPlatformImpl(Window window);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_LayoutManager")]
    static extern ILayoutManager GetLayoutManager(TopLevel topLevel);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "ExecuteInitialLayoutPass")]
    static extern void ExecuteInitialLayoutPass(ILayoutManager layoutManager);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Resized")]
    static extern void RendererResized(IRenderer renderer, Size size);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_arrangeBounds")]
    static extern ref Size GetArrangeBounds(Window window);

    #endregion

    #region UnsafeAccessors for IWindowImpl

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Resize")]
    static extern void ResizePlatformImpl(IWindowImpl? impl, Size clientSize, WindowResizeReason reason);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_ClientSize")]
    static extern Size GetPlatformClientSize(IWindowImpl? impl);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_FrameSize")]
    static extern Size? GetPlatformFrameSize(IWindowBaseImpl? impl);

    #endregion

    #region WindowResizedEventArgs Helper

    static WindowResizedEventArgs CreateWindowResizedEventArgs(Size clientSize, WindowResizeReason reason)
    {
        return (WindowResizedEventArgs)Activator.CreateInstance(
            typeof(WindowResizedEventArgs),
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [clientSize, reason],
            null)!;
    }

    #endregion

    #region MinMax Helper

    readonly record struct MinMaxValues(double MinWidth, double MaxWidth, double MinHeight, double MaxHeight);

    static MinMaxValues CreateMinMax(Layoutable control)
    {
        var minWidth = control.MinWidth;
        var maxWidth = control.MaxWidth;
        var minHeight = control.MinHeight;
        var maxHeight = control.MaxHeight;

        maxWidth = Math.Max(Math.Min(double.IsNaN(control.Width) ? double.PositiveInfinity : control.Width, maxWidth), minWidth);
        maxHeight = Math.Max(Math.Min(double.IsNaN(control.Height) ? double.PositiveInfinity : control.Height, maxHeight), minHeight);

        return new(minWidth, maxWidth, minHeight, maxHeight);
    }

    #endregion
}
