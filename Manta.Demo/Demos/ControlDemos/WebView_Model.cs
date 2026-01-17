using Manta.Demo.Reactive;
using Manta.Reactive.Views;
using Manta.WebView.Controls;

namespace Manta.Demo.Demos.ControlDemos;

[View<WebView>]
sealed class WebView_Model : DemoViewModel
{
    public static WebView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MWebView);
}
