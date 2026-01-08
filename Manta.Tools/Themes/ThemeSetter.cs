namespace Manta.Tools.Themes;

sealed class ThemeSetter
{
    public ThemeVariable Light { get; } = new(false);
    public ThemeVariable Dark { get; } = new(true);

    public bool IsLight { get; set; }

    public ThemeVariable Current => IsLight ? Light : Dark;
}
