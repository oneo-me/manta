using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ContextViewChildChild>]
sealed class ContextViewChildChild_Model : ViewModel
{
    public static ContextViewChildChild_Model Design => new();

    public ViewContext<ReactiveValue<bool>> TestBool => GetContext<ReactiveValue<bool>>("TestBool");
    public ViewContext<string> Data1 => GetContext<string>("Data1");
    public ViewContext<string> Data2 => GetContext<string>("Data2");
    public ViewContext<string> Data3 => GetContext<string>("Data3");
}
