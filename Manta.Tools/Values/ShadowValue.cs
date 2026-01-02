namespace Manta.Tools.Values;

public sealed class ShadowValue
{
    public string Value { get; set; } = string.Empty;

    public string ToString(string key)
    {
        return $"<BoxShadows x:Key=\"{{MShadowKey {key}}}\">{Value}</BoxShadows>";
    }

    public ShadowValue ToInset()
    {
        var parts = Value.Split(',').Select(p => "inset " + p.Trim());
        return new()
        {
            Value = string.Join(", ", parts)
        };
    }
}
