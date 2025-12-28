using Manta.Controls;
using Manta.Reactive.Views;
using Manta.Demo.Reactive;

namespace Manta.Demo.Demos.ControlDemos;

[View<MtChartView>]
sealed class LightChartView_Model : DemoViewModel
{
    public static LightChartView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Chart;
    public override string Header => nameof(MtChart);

    sealed class TestData(string month, string desktop, string mobile)
    {
        public string Month { get; } = month;
        public string Desktop { get; } = desktop;
        public string Mobile { get; } = mobile;
    }

    public string Title { get; }
    public MtChartData Data1 { get; }
    public MtChartData Data2 { get; }

    public LightChartView_Model()
    {
        Data1 = MtChartData.Create<TestData>([
            new("1", "186", "0"),
            new("2", "305", "0"),
            new("3", "237", "0"),
            new("4", "73", "0"),
            new("5", "209", "0"),
            new("6", "214", "0"),
            new("7", "156", "0"),
            new("8", "194", "0"),
            new("9", "263", "0"),
            new("10", "287", "0"),
            new("11", "335", "0"),
            new("12", "347", "0")
        ], x => x.Month, ("Desktop", x => x.Desktop));

        Data2 = MtChartData.Create<TestData>([
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
