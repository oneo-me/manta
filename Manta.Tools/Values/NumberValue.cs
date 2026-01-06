namespace Manta.Tools.Values;

public sealed class NumberValue
{
    public double Value { get; set; }

    public string ToString(string key)
    {
        return $"<system:Double x:Key=\"{{MSizeKey {key}}}\">{Value}</system:Double>";
    }
}
