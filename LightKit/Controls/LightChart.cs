using System.Linq.Expressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using LightKit.Themes.Values;

namespace LightKit.Controls;

public class LightChartData
{
    public class Legend(string data, IBrush brush)
    {
        public string Data { get; } = data;
        public IBrush Brush { get; } = brush;
    }

    public class Column(string name, IReadOnlyCollection<ColumnData> data)
    {
        public string Name { get; } = name;
        public IReadOnlyCollection<ColumnData> Data { get; } = data;
    }

    public class ColumnData(string name, double value, IBrush brush)
    {
        public string Name { get; } = name;
        public double Value { get; } = value;
        public IBrush Brush { get; } = brush;

        public double RenderRatio { get; internal set; }
    }

    static readonly OklchColor[] _defaultColors =
    [
        new() { L = 0.6, C = 0.25, H = 220 }, // Blue
        new() { L = 0.6, C = 0.25, H = 180 }, // Cyan
        new() { L = 0.7, C = 0.15, H = 90 }, // Lime
        new() { L = 0.7, C = 0.2, H = 29 }, // Orange
        new() { L = 0.65, C = 0.2, H = 140 }, // Green
        new() { L = 0.75, C = 0.15, H = 330 }, // Pink
        new() { L = 0.6, C = 0.3, H = 60 }, // Yellow
        new() { L = 0.65, C = 0.2, H = 260 }, // Purple
        new() { L = 0.7, C = 0.2, H = 10 }, // Red
        new() { L = 0.65, C = 0.2, H = 320 } // Magenta
    ];

    readonly List<Legend> _legends = new();
    readonly List<Column> _columns = new();

    public IReadOnlyList<Legend> Legends => _legends;
    public IReadOnlyList<Column> Columns => _columns;

    LightChartData(List<object> items, string columnMember, (string displayName, string dataMember)[] dataMembers)
    {
        // create colors;
        var colors = new Dictionary<int, IBrush>();
        for (var i = 0; i < dataMembers.Length; i++)
            colors[i] = new SolidColorBrush(_defaultColors[i % _defaultColors.Length].ToColor());

        // create legends
        for (var i = 0; i < dataMembers.Length; i++)
        {
            var member = dataMembers[i];
            _legends.Add(new(member.displayName, colors[i]));
        }

        // create columns
        foreach (var item in items)
        {
            var itemType = item.GetType();
            var columnName = itemType.GetProperty(columnMember)?.GetValue(item)?.ToString() ?? string.Empty;

            var data = new List<ColumnData>();

            for (var i = 0; i < dataMembers.Length; i++)
            {
                var member = dataMembers[i];
                var brush = colors[i];
                var valueObj = itemType.GetProperty(member.dataMember)?.GetValue(item);
                if (valueObj != null && double.TryParse(valueObj.ToString(), out var value))
                    data.Add(new(member.displayName, value, brush));
                else
                    data.Add(new(member.displayName, 0, brush));
            }

            _columns.Add(new(columnName, data));
        }

        // max value
        var maxValue = _columns.SelectMany(c => c.Data).Max(d => d.Value);
        foreach (var column in _columns)
        foreach (var data in column.Data)
            data.RenderRatio = data.Value / maxValue;

        Console.WriteLine($"Created {_columns.Count} columns.");
    }

    public static LightChartData Create<T>(IReadOnlyList<T> items, Expression<Func<T, object>> columnMember, params (string, Expression<Func<T, object>>)[] dataMembers)
    {
        return new(items.Cast<object>().ToList(), GetMemberName(columnMember), dataMembers.Select(x => (x.Item1, GetMemberName(x.Item2))).ToArray());
    }

    static string GetMemberName<TObject>(Expression<Func<TObject, object>> expression)
    {
        return expression.Body switch
        {
            MemberExpression m                              => m.Member.Name,
            UnaryExpression { Operand: MemberExpression m } => m.Member.Name,

            _ => throw new ArgumentException("Expression must be a member access")
        };
    }
}

[TemplatePart(nameof(PART_Columns), typeof(LightItems))]
public class LightChart : TemplatedControl
{
    public static readonly StyledProperty<object?> TitleProperty = AvaloniaProperty.Register<LightChart, object?>(
        nameof(Title));

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TitleDataTemplateProperty = AvaloniaProperty.Register<LightChart, IDataTemplate?>(
        nameof(TitleDataTemplate));

    public IDataTemplate? TitleDataTemplate
    {
        get => GetValue(TitleDataTemplateProperty);
        set => SetValue(TitleDataTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> ShowLegendsProperty = AvaloniaProperty.Register<LightChart, bool>(
        nameof(ShowLegends), true);

    public bool ShowLegends
    {
        get => GetValue(ShowLegendsProperty);
        set => SetValue(ShowLegendsProperty, value);
    }

    public static readonly StyledProperty<LightChartData?> DataProperty = AvaloniaProperty.Register<LightChart, LightChartData?>(
        nameof(Data));

    public LightChartData? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    LightItems? PART_Columns;
    LightChartToolTip? PART_ToolTip;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Columns = e.NameScope.Get<LightItems>(nameof(PART_Columns));
        PART_ToolTip = e.NameScope.Get<LightChartToolTip>(nameof(PART_ToolTip));

        PART_Columns.PointerEntered += ColumnsOnPointerEntered;
        PART_Columns.PointerExited += ColumnsOnPointerExited;
        PART_Columns.PointerMoved += ColumnsOnPointerMoved;
    }

    void ColumnsOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (PART_ToolTip is null)
            return;

        var point = e.GetPosition(this);
        var hitTestResult = this.InputHitTest(point);
        var column = (hitTestResult as Visual)?.GetVisualAncestors()
            .Prepend(hitTestResult as Visual)
            .OfType<LightChartColumn>()
            .FirstOrDefault();

        if (column is null)
            return;

        if (PART_ToolTip.Data != column.Data)
            PART_ToolTip.Data = column.Data;

        PART_ToolTip[Canvas.LeftProperty] = point.X + 10;
        PART_ToolTip[Canvas.TopProperty] = point.Y + 10;
    }

    void ColumnsOnPointerExited(object? sender, PointerEventArgs pointerEventArgs)
    {
        PART_ToolTip?.Opacity = 0;
    }

    void ColumnsOnPointerEntered(object? sender, PointerEventArgs pointerEventArgs)
    {
        PART_ToolTip?.Opacity = 1;
    }
}

public class LightChartColumn : TemplatedControl
{
    public static readonly StyledProperty<LightChartData.Column> DataProperty = AvaloniaProperty.Register<LightChartColumn, LightChartData.Column>(
        nameof(Data));

    public LightChartData.Column Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}

public class LightChartToolTip : TemplatedControl
{
    public static readonly StyledProperty<LightChartData.Column> DataProperty = AvaloniaProperty.Register<LightChartColumn, LightChartData.Column>(
        nameof(Data));

    public LightChartData.Column Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}

public class LightChartBarPanel : Panel
{
    public static readonly StyledProperty<double> RatioProperty = AvaloniaProperty.Register<LightChartBarPanel, double>(
        nameof(Ratio));

    public double Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var panelHeight = double.IsPositiveInfinity(availableSize.Height) ? 0.0 : availableSize.Height;
        var childConstraintHeight = panelHeight * Ratio;
        var childConstraint = new Size(availableSize.Width, childConstraintHeight);

        foreach (var child in Children)
            child.Measure(childConstraint);

        return new(availableSize.Width, panelHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var arrangeRect = new Rect(0, finalSize.Height * (1 - Ratio), finalSize.Width, finalSize.Height * Ratio);

        foreach (var child in Children)
            child.Arrange(arrangeRect);

        return finalSize;
    }
}

public class LightChartLegend : TemplatedControl
{
    public static readonly StyledProperty<IBrush?> BrushProperty = AvaloniaProperty.Register<LightChartLegend, IBrush?>(
        nameof(Brush));

    public IBrush? Brush
    {
        get => GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    public static readonly StyledProperty<string?> DataProperty = AvaloniaProperty.Register<LightChartLegend, string?>(
        nameof(Data));

    public string? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
