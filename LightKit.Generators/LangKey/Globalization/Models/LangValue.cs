using Microsoft.CodeAnalysis;

namespace LightKit.Generators.LangKey.Globalization.Models;

sealed class LangValue
{
    public string Key { get; set; } = string.Empty;
    public Location KeyLocation { get; set; } = Location.None;
    public string Value { get; set; } = string.Empty;
}
