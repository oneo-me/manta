using Axe.Reactive.Views;
using AxeDemo.Controls;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<MultiLanguageView>]
sealed class MultiLanguageView_Model : HeaderRegionModel
{
    public static MultiLanguageView_Model Design => new();
    public override IconKey Icon => IconKey.MultiLanguage;
    public override LangKey Header => LangKey.MultiLanguage;
}
