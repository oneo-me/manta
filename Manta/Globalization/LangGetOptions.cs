using Manta.Globalization.Core;

namespace Manta.Globalization;

public readonly struct LangGetOptions(string langKey, params object?[]? args)
{
    public string LangKey { get; } = langKey;
    public object?[]? Args { get; } = args;

    public LangType? LangType { get; init; }
    public string? DefValue { get; init; }
}
