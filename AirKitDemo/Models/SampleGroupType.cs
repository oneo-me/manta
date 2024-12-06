using AirKitDemo.Langs;

namespace AirKitDemo.Models;

enum SampleGroupType
{
    [SampleGroup(LangKey.GroupType_Themes)]
    Themes,

    [SampleGroup(LangKey.GroupType_Reactive)]
    Reactive,

    [SampleGroup(LangKey.GroupType_Controls)]
    Controls
}
