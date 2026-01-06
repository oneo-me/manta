using Manta.Charts.Controls;
using Manta.Charts.Models;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.ControlDemos;

[View<ChartView>]
sealed class ChartView_Model : DemoViewModel
{
    public static ChartView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MChart);

    public BarChartData BarData { get; }
    public BarChartData LineData { get; }
    public PieChartData PieData { get; }

    public ChartView_Model()
    {
        var data = new List<(string month, double desktop, double mobile)>
        {
            ("January", 186, 80),
            ("February", 305, 200),
            ("March", 237, 120),
            ("April", 73, 190),
            ("May", 209, 130),
            ("June", 214, 140)
        };

        BarData = new()
        {
            Items = data.Select(x => new BarChartDataItem
            {
                Category = x.month,
                Values = new List<ChartDataValue>
                {
                    new() { Name = "Desktop", Value = x.desktop },
                    new() { Name = "Mobile", Value = x.mobile }
                }
            }).ToList()
        };

        LineData = new()
        {
            Type = BarChartType.Line,
            Items = data.Select(x => new BarChartDataItem
            {
                Category = x.month,
                Values = new List<ChartDataValue>
                {
                    new() { Name = "Desktop", Value = x.desktop },
                    new() { Name = "Mobile", Value = x.mobile }
                }
            }).ToList()
        };

        PieData = new()
        {
            Values = data.Select(x => new ChartDataValue
            {
                Name = x.month,
                Value = x.desktop + x.mobile
            }).ToList()
        };
    }
}
