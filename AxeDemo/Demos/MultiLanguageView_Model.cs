using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<MultiLanguageView>]
sealed class MultiLanguageView_Model : HeaderRegionModel
{
    public static MultiLanguageView_Model Design => new();
    public override IconSource Icon { get; } = Assets.GetIconSource("Icons/Planet--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.MultiLanguage;
}
