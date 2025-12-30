using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Manta.Charts.Models;
using Manta.Charts.Renders;

namespace Manta.Charts.Controls;

public class MChart : TemplatedControl
{
    MChartTooltipHost? _tooltipHost;

    static MChart()
    {
        DataProperty.Changed.AddClassHandler<MChart>((chart, _) => chart.UpdateLegendItems());
        TitleProperty.Changed.AddClassHandler<MChart>((chart, _) => chart.UpdatePseudoClasses());
        ShowLegendProperty.Changed.AddClassHandler<MChart>((chart, _) => chart.UpdatePseudoClasses());
    }

    public static readonly StyledProperty<ChartDataBase?> DataProperty =
        AvaloniaProperty.Register<MChart, ChartDataBase?>(nameof(Data));

    public ChartDataBase? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<MChart, double>(nameof(Spacing));

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<MChart, string>(nameof(Title), string.Empty);

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<bool> ShowLegendProperty =
        AvaloniaProperty.Register<MChart, bool>(nameof(ShowLegend), true);

    public bool ShowLegend
    {
        get => GetValue(ShowLegendProperty);
        set => SetValue(ShowLegendProperty, value);
    }

    public static readonly StyledProperty<ChartLegendOrientation> LegendOrientationProperty =
        AvaloniaProperty.Register<MChart, ChartLegendOrientation>(nameof(LegendOrientation));

    public ChartLegendOrientation LegendOrientation
    {
        get => GetValue(LegendOrientationProperty);
        set => SetValue(LegendOrientationProperty, value);
    }

    public static readonly DirectProperty<MChart, ObservableCollection<ChartLegendItem>> LegendItemsProperty =
        AvaloniaProperty.RegisterDirect<MChart, ObservableCollection<ChartLegendItem>>(
            nameof(LegendItems),
            o => o.LegendItems,
            (o, v) => o.LegendItems = v);

    ObservableCollection<ChartLegendItem> _legendItems = [];

    public ObservableCollection<ChartLegendItem> LegendItems
    {
        get => _legendItems;
        private set => SetAndRaise(LegendItemsProperty, ref _legendItems, value);
    }

    void UpdateLegendItems()
    {
        _legendItems.Clear();

        switch (Data)
        {
            case BarChartData barData:
                var seriesNames = barData.Items.FirstOrDefault()?.Values.Select(v => v.Name).ToList() ?? [];
                for (var i = 0; i < seriesNames.Count; i++)
                    _legendItems.Add(new()
                    {
                        Name = seriesNames[i],
                        Color = ChartRenderContext.DefaultColors[i % ChartRenderContext.DefaultColors.Length]
                    });

                break;

            case PieChartData pieData:
                for (var i = 0; i < pieData.Values.Count; i++)
                {
                    var value = pieData.Values.ElementAt(i);
                    _legendItems.Add(new()
                    {
                        Name = value.Name,
                        Color = value.CustomColor ?? ChartRenderContext.DefaultColors[i % ChartRenderContext.DefaultColors.Length]
                    });
                }

                break;
        }
    }

    public static readonly StyledProperty<bool> ShowTooltipProperty =
        AvaloniaProperty.Register<MChart, bool>(nameof(ShowTooltip), true);

    public bool ShowTooltip
    {
        get => GetValue(ShowTooltipProperty);
        set => SetValue(ShowTooltipProperty, value);
    }

    public static readonly DirectProperty<MChart, bool> IsTooltipOpenProperty =
        AvaloniaProperty.RegisterDirect<MChart, bool>(nameof(IsTooltipOpen), o => o.IsTooltipOpen);

    public bool IsTooltipOpen
    {
        get;
        private set => SetAndRaise(IsTooltipOpenProperty, ref field, value);
    }

    public static readonly DirectProperty<MChart, Point> TooltipPositionProperty =
        AvaloniaProperty.RegisterDirect<MChart, Point>(nameof(TooltipPosition), o => o.TooltipPosition);

    public Point TooltipPosition
    {
        get;
        private set => SetAndRaise(TooltipPositionProperty, ref field, value);
    }

    public static readonly DirectProperty<MChart, string> TooltipHeaderProperty =
        AvaloniaProperty.RegisterDirect<MChart, string>(nameof(TooltipHeader), o => o.TooltipHeader);

    public string TooltipHeader
    {
        get;
        private set => SetAndRaise(TooltipHeaderProperty, ref field, value);
    } = string.Empty;

    public static readonly DirectProperty<MChart, ObservableCollection<ChartTooltipLine>> TooltipLinesProperty =
        AvaloniaProperty.RegisterDirect<MChart, ObservableCollection<ChartTooltipLine>>(nameof(TooltipLines), o => o.TooltipLines);

    public ObservableCollection<ChartTooltipLine> TooltipLines
    {
        get;
        private set => SetAndRaise(TooltipLinesProperty, ref field, value);
    } =
        [];

    public MChart()
    {
        AddHandler(MChartRender.HoverChangedEvent, OnHoverChanged);
        UpdatePseudoClasses();
    }

    void UpdatePseudoClasses()
    {
        if (string.IsNullOrEmpty(Title))
            PseudoClasses.Add(":empty-title");
        else
            PseudoClasses.Remove(":empty-title");

        if (ShowLegend)
            PseudoClasses.Add(":show-legend");
        else
            PseudoClasses.Remove(":show-legend");
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _tooltipHost = e.NameScope.Find("PART_TooltipHost") as MChartTooltipHost;
    }

    void OnHoverChanged(object? sender, ChartHoverEventArgs e)
    {
        if (!ShowTooltip)
        {
            IsTooltipOpen = false;
            return;
        }

        IsTooltipOpen = e.GroupIndex >= 0;
        TooltipPosition = e.Position;
        TooltipHeader = e.Header;
        TooltipLines = e.Lines;

        if (_tooltipHost is not null)
        {
            _tooltipHost.IsOpen = IsTooltipOpen;
            _tooltipHost.Position = TooltipPosition;
            _tooltipHost.Header = TooltipHeader;
            _tooltipHost.Lines = TooltipLines;
        }
    }
}
