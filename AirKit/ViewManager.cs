using AirKit.Extensions;

namespace AirKit;

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
        var viewModel = Service.CreateInstance<T>(typeof(T));
        func?.Invoke(viewModel);
        OpenViewCore(viewModel);
    }

    public void OpenView<T>(Type type, Action<T>? func = null) where T : ViewModel
    {
        var viewModel = Service.CreateInstance<T>(type);
        func?.Invoke(viewModel);
        OpenViewCore(viewModel);
    }

    public void OpenView(Type type)
    {
        var viewModel = Service.CreateInstance<ViewModel>(type);
        OpenViewCore(viewModel);
    }

    public void Dispose()
    {
        ClearView();
    }
}
