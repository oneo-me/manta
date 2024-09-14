using AirKit.Reactive.Attributes;
using AirKit.Reactive.Extensions;
using R3;

namespace AirKit.Reactive.Demo.Views;

[View<DemoView>]
public class DemoView_Model : ViewModel
{
    public ReactiveCommand<Unit> DisposeCommand { get; }

    public DemoView_Model()
    {
        DisposeCommand = new(_ =>
        {
            this.GetWindow().Content = null;

            for (var i = 0; i < 100; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        });
    }
}
