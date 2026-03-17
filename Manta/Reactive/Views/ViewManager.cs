using Manta.Extensions;
using Manta.Reactive.Extensions;

namespace Manta.Reactive.Views;

public sealed class ViewManager : Model, IDisposable
{
    public ViewModel? Current
    {
        get;
        private set => SetValue(ref field, value);
    }

    public void ClearView()
    {
        var current = Current;
        Current = null;
        current?.DisposeIfDisposable();
    }

    void OpenViewCore(ViewModel viewModel)
    {
        ClearView();
        Current = viewModel;
    }

    public void OpenView<T>(Func<T> func) where T : ViewModel
    {
        var viewModel = func();
        OpenViewCore(viewModel);
    }

    public void OpenView<T>(Action<T>? func = null) where T : ViewModel
    {
        var viewModel = typeof(T).CreateInstance<T>();
        func?.Invoke(viewModel);
        OpenViewCore(viewModel);
    }

    public void OpenView<T>(Type type, Action<T>? func = null) where T : ViewModel
    {
        var viewModel = type.CreateInstance<T>();
        func?.Invoke(viewModel);
        OpenViewCore(viewModel);
    }

    public void OpenView(Type type)
    {
        var viewModel = type.CreateInstance<ViewModel>();
        OpenViewCore(viewModel);
    }

    public void Dispose()
    {
        ClearView();
    }
}
