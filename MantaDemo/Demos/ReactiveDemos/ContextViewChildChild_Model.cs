using Manta.Controls;
using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

sealed class ContextViewChildChild_Model : ViewModel
{
    protected override MView CreateView()
    {
        return new ContextViewChildChild();
    }

    public ViewContext<ReactiveValue<bool>> TestBool => GetContext<ReactiveValue<bool>>("TestBool");
    public ViewContext<string> Data1 => GetContext<string>("Data1");
    public ViewContext<string> Data2 => GetContext<string>("Data2");
    public ViewContext<string> Data3 => GetContext<string>("Data3");
}
