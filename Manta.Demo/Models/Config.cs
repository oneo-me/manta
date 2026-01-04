using Manta.Demo.Reactive;
using Manta.Globalization.Core;

namespace Manta.Demo.Models;

sealed class Config
{
    public LangType Language { get; set; } = LangType.en;
    public string Current { get; set; } = string.Empty;
    public Dictionary<DemoGroup, string> LastDemos { get; set; } = new();
    public double Width { get; set; } = 800;
    public double Height { get; set; } = 600;
    public ToolConfig Tools { get; set; } = new();
}
