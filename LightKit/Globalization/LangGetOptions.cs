using LightKit.Globalization.Core;

namespace LightKit.Globalization;

public readonly struct LangGetOptions(string langKey)
{
    public string LangKey { get; } = langKey;

    public LangType? LangType { get; init; }
    public string? DefValue { get; init; }
    public object?[]? Args { get; init; }
}
