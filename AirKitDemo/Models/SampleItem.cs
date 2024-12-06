using AirKitDemo.Langs;

namespace AirKitDemo.Models;

sealed class SampleItem(LangKey nameKey, Type? type = null)
{
    public LangKey NameKey { get; } = nameKey;
    public Type? Type { get; } = type;
    public bool IsGroup => Type is null;
}
