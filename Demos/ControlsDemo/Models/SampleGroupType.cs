using ControlsDemo.Langs;

namespace ControlsDemo.Models;

enum SampleGroupType
{
    [SampleGroup(LangKey.GroupType_Themes)]
    Themes,

    [SampleGroup(LangKey.GroupType_Reactive)]
    Reactive,

    [SampleGroup(LangKey.GroupType_Controls)]
    Controls
}
