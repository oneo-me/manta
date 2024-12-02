namespace AirKitDemo.Models;

sealed class SampleItem(string name, Type? type = null)
{
    public string Name { get; } = name;
    public Type? Type { get; } = type;
    public bool IsGroup => Type is null;
}
