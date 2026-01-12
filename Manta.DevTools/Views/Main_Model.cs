using Avalonia.Collections;
using Manta.Controls;
using Manta.DevTools.Models;
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

    public AvaloniaList<ViewNode> Root { get; } = [];

    public Main_Model(DevToolsAttacher? attacher)
    {
        _attacher = attacher;

        if (_attacher?.SourceWindow.Content is not MView { DataContext: ViewModel viewModel } view)
            return;

        Root.Add(new(view, viewModel));
    }

    public void ShowOrHideDevTools()
    {
        _attacher?.ShowOrHideDevTools();
    }
}
