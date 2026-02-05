using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Rendering;
using Avalonia.Styling;
using Manta.Controls;
using Manta.DevTools.Models;
using Manta.Extensions;
using Manta.Reactive.Commands;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<Main>]
sealed class Main_Model : ViewModel
{
    public static Main_Model Design => new(null!);

    readonly DevToolsAttacher? _attacher;

    public bool IsVisible
    {
        get;
        set => SetValue(ref field, value);
    } = true;

    public AvaloniaList<TreeNode> Root { get; } = [];
    public ReactiveValue<TreeNode?> SelectedNode { get; } = new(null);

    public Main_Model(DevToolsAttacher? attacher)
    {
        _attacher = attacher;

        if (_attacher?.SourceWindow.Content is not MView { DataContext: ViewModel viewModel } view)
            return;

        Root.Add(new(null, view, viewModel));

        // Initialize debug overlays
        EnabledFps = _attacher.Options.EnableFpsByDefault;
        EnabledDirtyRects = _attacher.Options.EnableDirtyRectsByDefault;
        EnabledTimeGraph = _attacher.Options.EnableTimeGraphByDefault;

        SetContext(nameof(Root), Root);
        SetContext(nameof(SelectedNode), SelectedNode);
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();
        view.TitleBar
            .BindTitleBarEvents()
            .DisposeWith(disposables);
    }

    public CommandWarper HideDevToolsCommand => GetCommand(() =>
    {
        _attacher?.ShowOrHideDevTools();
    });

    public bool EnabledFps
    {
        get;
        set
        {
            if (SetValue(ref field, value))
                if (_attacher is not null)
                    if (value)
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays |= RendererDebugOverlays.Fps;
                    else
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays &= ~RendererDebugOverlays.Fps;
        }
    }

    public bool EnabledDirtyRects
    {
        get;
        set
        {
            if (SetValue(ref field, value))
                if (_attacher is not null)
                    if (value)
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays |= RendererDebugOverlays.DirtyRects;
                    else
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays &= ~RendererDebugOverlays.DirtyRects;
        }
    }

    public bool EnabledTimeGraph
    {
        get;
        set
        {
            if (SetValue(ref field, value))
                if (_attacher is not null)
                    if (value)
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays |= RendererDebugOverlays.LayoutTimeGraph | RendererDebugOverlays.RenderTimeGraph;
                    else
                        _attacher.SourceWindow.RendererDiagnostics.DebugOverlays &= ~(RendererDebugOverlays.LayoutTimeGraph | RendererDebugOverlays.RenderTimeGraph);
        }
    }

    public string ThemeButtonText
    {
        get;
        set => SetValue(ref field, value);
    } = GetThemeText(Application.Current?.RequestedThemeVariant);

    public CommandWarper ToggleThemeCommand => GetCommand(() =>
    {
        ThemeButtonText = GetThemeText(MApp.SwitchTheme());
    });

    static string GetThemeText(ThemeVariant? variant) => variant == ThemeVariant.Default ? "System" : variant == ThemeVariant.Dark ? "Dark" : "Light";
}
