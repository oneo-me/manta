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

    public void OpenView<T>() where T : ViewModel, new()
    {
        var viewModel = Service.CreateInstance<T>();
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
