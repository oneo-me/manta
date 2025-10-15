using Avalonia.Controls;

namespace LightKit.Reactive.Views;

public class ViewTemplate<T>(Func<T, Control> build) : ViewTemplate
{
    public ViewTemplate(Func<T, ViewModel> build) : this(data => build(data).CreateView())
    {
    }

    public override bool Match(object? data)
    {
        return data is T;
    }

    public override Control? Build(object? param)
    {
        return param is not T data ? null : build(data);
    }
}
