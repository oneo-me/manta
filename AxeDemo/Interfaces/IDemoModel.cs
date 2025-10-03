using AxeDemo.Langs;

namespace AxeDemo.Interfaces;

enum DemoGroup
{
    Styles,
    Controls,
    Reactive,
    MultiLanguage
}

interface IDemoModel
{
    LangKey Header { get; }
    DemoGroup Group { get; }
}
