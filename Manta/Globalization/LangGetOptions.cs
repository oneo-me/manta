using Manta.Globalization.Core;

namespace Manta.Globalization;

public readonly struct LangGetOptions(string langKey, params object?[]? args) : IEquatable<LangGetOptions>
{
    readonly object?[]? _args = args;

    public string LangKey { get; } = langKey;
    public LangType? LangType { get; init; }
    public string? DefValue { get; init; }

    public object?[]? GetArgs()
    {
        return _args;
    }

    public bool Equals(LangGetOptions other)
    {
        return Equals(_args, other._args) && LangKey == other.LangKey && LangType == other.LangType && DefValue == other.DefValue;
    }

    public override bool Equals(object? obj)
    {
        return obj is LangGetOptions other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_args, LangKey, LangType, DefValue);
    }

    public static bool operator ==(LangGetOptions left, LangGetOptions right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LangGetOptions left, LangGetOptions right)
    {
        return !(left == right);
    }
}
