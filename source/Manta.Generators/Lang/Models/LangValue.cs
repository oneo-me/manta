using Microsoft.CodeAnalysis;

namespace Manta.Generators.Lang.Models;

sealed class LangValue
{
    public string Key { get; set; } = string.Empty;
    public Location KeyLocation { get; set; } = Location.None;
    public string Value { get; set; } = string.Empty;
}
