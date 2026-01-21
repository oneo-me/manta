using Manta.Reactive.Views;
using Manta.WebView.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<WebView>]
sealed class WebView_Model : DemoViewModel
{
    public static WebView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MWebView);
}
