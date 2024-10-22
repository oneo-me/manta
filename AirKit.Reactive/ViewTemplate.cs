using AirKit.Reactive.Extensions;
using AirKit.Reactive.Logging;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace AirKit.Reactive;

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
        var result = _build?.Invoke();

        Log.Verbose("ViewModel", "Build view {viewType} with {paramType}", result?.GetType().Name, param?.GetType().Name);

        return result;
    }
}
