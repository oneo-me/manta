namespace AirKit.Themes.Enums;

[Flags]
public enum ThicknessFilter
{
    None = 0,

    Left = 1 << 0,
    Top = 1 << 1,
    Right = 1 << 2,
    Bottom = 1 << 3,

    All = Left | Top | Right | Bottom
}
