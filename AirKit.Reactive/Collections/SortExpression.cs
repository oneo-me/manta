namespace AirKit.Reactive.Collections;

sealed class SortExpression<T>(Func<T, IComparable> expression, SortDirection direction = SortDirection.Ascending)
{
    public SortDirection Direction { get; } = direction;
    public Func<T, IComparable?> Expression { get; } = expression;
}
