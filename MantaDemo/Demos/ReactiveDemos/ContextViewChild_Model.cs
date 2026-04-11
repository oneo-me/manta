using Manta.Controls;
using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ContextViewChild>]
sealed partial class ContextViewChild_Model : ViewModel
{
    public ViewContext<ReactiveValue<bool>> TestBool => GetContext<ReactiveValue<bool>>("TestBool");
    public ViewContext<string> Data1 => GetContext<string>("Data1");
    public ViewContext<string> Data2 => GetContext<string>("Data2");

    public static string Data3 => "ContextViewChild Data3";

    public ContextViewChild_Model()
    {
        SetContext(nameof(Data2), "ContextViewChild Data2");
        SetContext(nameof(Data3), Data3);
    }
}
