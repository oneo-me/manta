using Microsoft.CodeAnalysis;

namespace LightKit.Generators.I18N.Models;

sealed class LangData(AdditionalText file)
{
    public AdditionalText File { get; } = file;
    public Dictionary<string, LangValue> Values { get; } = new();
    public Location Location { get; } = Location.Create(file.Path, new(0, 0), new());
}
