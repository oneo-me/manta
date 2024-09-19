using System.Diagnostics.CodeAnalysis;

namespace AirKit.Reactive.Collections;

[SuppressMessage("Design", "CA1000:不要在泛型类型中声明静态成员")]
public class SortExpressionComparer<T> : IComparer<T>
{
    readonly List<SortExpression<T>> _items = [];

    public static SortExpressionComparer<T> Ascending(Func<T, IComparable> expression)
    {
        return new SortExpressionComparer<T>().ThenByAscending(expression);
    }

    public static SortExpressionComparer<T> Descending(Func<T, IComparable> expression)
    {
        return new SortExpressionComparer<T>().ThenByDescending(expression);
    }

    public SortExpressionComparer<T> ThenByAscending(Func<T, IComparable> expression)
    {
        _items.Add(new(expression));
        return this;
    }

    public SortExpressionComparer<T> ThenByDescending(Func<T, IComparable> expression)
    {
        _items.Add(new(expression, SortDirection.Descending));
        return this;
    }

    public int Compare(T? x, T? y)
    {
        foreach (var item in _items)
        {
            if (x is null && y is null)
                continue;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            var xValue = item.Expression(x);
            var yValue = item.Expression(y);

            if (xValue is null && yValue is null)
                continue;

            if (xValue is null)
                return -1;

            if (yValue is null)
                return 1;

            var result = xValue.CompareTo(yValue);
            if (result == 0)
                continue;

            return item.Direction == SortDirection.Ascending ? result : -result;
        }

        return 0;
    }
}
