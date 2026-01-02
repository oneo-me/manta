namespace Manta.Tools.Themes;

sealed class ThemeSetter
{
    public ThemeVariable Light { get; } = new();
    public ThemeVariable Dark { get; } = new();

    public bool IsLight { get; set; }

    public ThemeVariable Current => IsLight ? Light : Dark;
}
