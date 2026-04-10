using Microsoft.CodeAnalysis;

namespace Manta.Generators.Lang.Globalization.Models;

sealed class LangData(AdditionalText file)
{
    public AdditionalText File { get; } = file;
    public Dictionary<string, LangValue> Values { get; } = new();
    public Location Location { get; } = Location.Create(file.Path, new(0, 0), new());
}
