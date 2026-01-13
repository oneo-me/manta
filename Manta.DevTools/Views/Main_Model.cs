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

    public AvaloniaList<ViewContext> Contexts { get; } = [];
    public AvaloniaList<ViewNode> Root { get; } = [];

    public ViewNode? SelectedItem
    {
        get;
        set
        {
            if (SetValue(ref field, value))
            {
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

    static ViewContext? GetViewContext(ViewNode? node)
    {
        if (node is null)
            return null;

        if (node.ViewModel.Context is null)
            return null;

        var context = new ViewContext(node.Name);

        foreach (var (key, value) in node.ViewModel.Context)
            context.Items.Add(new(key, value));

        return context;
    }

    public Main_Model(DevToolsAttacher? attacher)
    {
        _attacher = attacher;

        if (_attacher?.SourceWindow.Content is not MView { DataContext: ViewModel viewModel } view)
            return;

        Root.Add(new(null, view, viewModel));
    }

    public void ShowOrHideDevTools()
    {
        _attacher?.ShowOrHideDevTools();
    }
}
