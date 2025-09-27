namespace Axe.Icons.Generator.Models;

sealed class IconData(string key)
{
    public string Key { get; } = key;
    public double IconSize { get; set; } = 0d;
    public List<string> Paths { get; } = [];
}
