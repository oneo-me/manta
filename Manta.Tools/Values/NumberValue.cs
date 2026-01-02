namespace Manta.Tools.Values;

public sealed class NumberValue
{
    public double Value { get; set; }

    public string ToString(string prefix, string key)
    {
        return $"<system:Double x:Key=\"{{M{prefix}Key {key}}}\">{Value}</system:Double>";
    }
}
