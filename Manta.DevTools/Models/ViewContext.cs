using Avalonia.Collections;

namespace Manta.DevTools.Models;

class ViewContext(string name)
{
    public string Name { get; } = name;
    public AvaloniaList<ViewContextItem> Items { get; } = [];
}
