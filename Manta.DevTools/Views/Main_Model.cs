using System.Reactive.Disposables;
using Avalonia.Collections;
using Avalonia.Rendering;
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

    public AvaloniaList<ViewContext> Contexts { get; } = [];
    public AvaloniaList<ViewNode> Root { get; } = [];

    public ViewNode? SelectedItem
    {
        get;
        set
        {
            if (SetValue(ref field, value))
            {
                for (var i = LogicalRoot.Count - 1; i >= 0; i--)
                {
                    var item = LogicalRoot[i];
                    LogicalRoot.RemoveAt(i);
                    item.Dispose();
                }

                if (value is not null)
                    LogicalRoot.Add(new(null, value.View));

                // Update Contexts
                Contexts.Clear();

                var current = value;

                while (current is not null)
                {
                    var currentContext = GetViewContext(current);
                    if (currentContext is not null)
                        Contexts.Add(currentContext);

                    current = current.Parent;
                }
            }
        }
    }

    public AvaloniaList<LogicalNode> LogicalRoot { get; } = [];

    static ViewContext? GetViewContext(ViewNode? node)
    {
        if (node is null)
            return null;

        var keyValuePairs = node.ViewModel.InnerGetContextItems().ToList();
        if (keyValuePairs.Count == 0)
            return null;

        var context = new ViewContext(node.Header);

        foreach (var (key, value) in keyValuePairs)
            context.Items.Add(new(key, value));

        return context;
    }

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
}
