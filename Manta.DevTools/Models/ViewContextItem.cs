namespace Manta.DevTools.Models;

sealed class ViewContextItem(string key, object? value)
{
    public string Key { get; } = key;
    public object? Value { get; } = value;
}
