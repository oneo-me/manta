using Manta.Controls;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ContextView>]
sealed partial class ContextView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_ViewContext;

    public ReactiveValue<bool> TestBool { get; } = new(true);
    public static string Data1 => "Hello, World!";
    public static string Data2 => "Goodbye, World!";

    public ContextView_Model()
    {
        SetContext(nameof(TestBool), TestBool);
        SetContext(nameof(Data1), Data1);
        SetContext(nameof(Data2), Data2);
    }
}
