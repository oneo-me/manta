using Avalonia;
using Manta.Extensions;

namespace Manta.Themes.Variables;

[Flags]
public enum MThicknessFilter
{
    None = 0,

    Left = 1 << 0,
    Top = 1 << 1,
    Right = 1 << 2,
    Bottom = 1 << 3,

    All = Left | Top | Right | Bottom
}

public sealed class MThickness(MSizeVariant variant) : MSizeBase
{
    public MSizeVariant Offset { get; init; } = MSizeVariant.S0;
    public MSizeVariant OffsetNegative { get; init; } = MSizeVariant.S0;
    public bool Reverse { get; init; }

    MThicknessFilter _filter = MThicknessFilter.All;

    public string Filter
    {
        set => _ = value.TryParseEnum(out _filter, '|');
    }

    public MSizeVariant? Vertical { get; init; }

    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (values[0] is double number)
            size = number;

        var offsetSize = CalculateSize(size, Offset);
        var offsetNegativeSize = CalculateSize(size, OffsetNegative);
        var horizontalSize = CalculateSize(size, variant) + offsetSize - offsetNegativeSize;
        var verticalSize = Vertical.HasValue ? CalculateSize(size, Vertical.Value) + offsetSize - offsetNegativeSize : horizontalSize;
        if (size <= 0 && verticalSize <= 0)
            return default(Thickness);

        if (Reverse)
        {
            horizontalSize = -horizontalSize;
            verticalSize = -verticalSize;
        }

        return new Thickness(
            _filter.HasFlag(MThicknessFilter.Left, horizontalSize),
            _filter.HasFlag(MThicknessFilter.Top, verticalSize),
            _filter.HasFlag(MThicknessFilter.Right, horizontalSize),
            _filter.HasFlag(MThicknessFilter.Bottom, verticalSize)
        );
    }
}
