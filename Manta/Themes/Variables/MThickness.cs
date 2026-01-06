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
    MThicknessFilter _filter = MThicknessFilter.All;

    public string Filter
    {
        set => value.TryParseEnum(out _filter, '|');
    }

    public MSizeVariant? Vertical { get; init; }

    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (values[0] is double number)
            size = number;

        var horizontalSize = CalculateSize(size, variant);
        var verticalSize = Vertical.HasValue ? CalculateSize(size, Vertical.Value) : horizontalSize;
        if (size <= 0 && verticalSize <= 0)
            return default(Thickness);

        return new Thickness(
            _filter.HasFlag(MThicknessFilter.Left, horizontalSize),
            _filter.HasFlag(MThicknessFilter.Top, verticalSize),
            _filter.HasFlag(MThicknessFilter.Right, horizontalSize),
            _filter.HasFlag(MThicknessFilter.Bottom, verticalSize)
        );
    }
}
