using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<MultiLanguageView>]
sealed class MultiLanguageView_Model : HeaderRegionModel
{
    public static MultiLanguageView_Model Design => new();
    public override LangKey Header => LangKey.MultiLanguage;
}
