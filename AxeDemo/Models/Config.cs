using Axe.I18N.Core;

namespace AxeDemo.Models;

sealed class Config
{
    public LangType Language { get; set; } = LangType.en;
    public string Current { get; set; } = string.Empty;
}
