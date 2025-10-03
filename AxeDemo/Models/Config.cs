using Axe.I18N.Core;
using AxeDemo.Reactive;

namespace AxeDemo.Models;

sealed class Config
{
    public LangType Language { get; set; } = LangType.en;
    public string Current { get; set; } = string.Empty;
    public Dictionary<DemoGroup, string> LastDemos { get; set; } = new();
}
