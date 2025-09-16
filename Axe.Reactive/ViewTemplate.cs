using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Axe.Reactive;

public class ViewTemplate : IDataTemplate
{
    readonly Func<Control>? _build;

    internal ViewTemplate()
    {
    }

    public ViewTemplate(Func<ViewModel> build) : this(() => build().CreateView())
    {
    }

    public ViewTemplate(Func<Control> build)
    {
        _build = build;
    }

    public virtual bool Match(object? data)
    {
        return true;
    }

    public virtual Control? Build(object? param)
    {
        return _build?.Invoke();
    }
}
