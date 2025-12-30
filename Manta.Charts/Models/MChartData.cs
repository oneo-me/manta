using System.Linq.Expressions;
using Avalonia.Media;
using Manta.Themes.Values;

namespace Manta.Charts.Models;

public class MChartData
{
    public sealed class Legend(string data, IBrush brush)
    {
        public string Data { get; } = data;
        public IBrush Brush { get; } = brush;
    }

    public sealed class Column(string name, IReadOnlyCollection<ColumnData> data)
    {
        public string Name { get; } = name;
        public IReadOnlyCollection<ColumnData> Data { get; } = data;
    }

    public sealed class ColumnData(string name, double value, IBrush brush)
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

    MChartData(List<object> items, string columnMember, (string displayName, string dataMember)[] dataMembers)
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
            var iteMype = item.GetType();
            var columnName = iteMype.GetProperty(columnMember)?.GetValue(item)?.ToString() ?? string.Empty;

            var data = new List<ColumnData>();

            for (var i = 0; i < dataMembers.Length; i++)
            {
                var member = dataMembers[i];
                var brush = colors[i];
                var valueObj = iteMype.GetProperty(member.dataMember)?.GetValue(item);
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
    }

    public static MChartData Create<T>(IReadOnlyList<T> items, Expression<Func<T, object>> columnMember, params (string, Expression<Func<T, object>>)[] dataMembers)
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
