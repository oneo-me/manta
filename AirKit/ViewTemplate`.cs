using AirKit.Extensions;
using AirKit.Logging;
using Avalonia.Controls;

namespace AirKit;

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
        var result = param is not T data ? null : build(data);

        Log.Verbose("ViewModel", "Build view {viewType} with {paramType} data", result?.GetType().Name, param?.GetType().Name);

        return result;
    }
}
