using LightKit.Controls;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<LightChartView>]
sealed class LightChartView_Model : DemoViewModel
{
    public static LightChartView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override string Header => nameof(LightChart);

    sealed class TestData(string month, string desktop, string mobile)
    {
        public string Month { get; } = month;
        public string Desktop { get; } = desktop;
        public string Mobile { get; } = mobile;
    }

    public string Title { get; }
    public LightChartData Data { get; }

    public LightChartView_Model()
    {
        Data = LightChartData.Create<TestData>([
            new("January", "186", "80"),
            new("February", "305", "200"),
            new("March", "237", "120"),
            new("April", "73", "190"),
            new("May", "209", "130"),
            new("June", "214", "140")
        ], x => x.Month, ("Desktop", x => x.Desktop), ("Mobile", x => x.Mobile));

        Title = "TestData items";
    }
}
