using Manta.Globalization;
using Manta.Globalization.Core;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaSample.Langs;
using MantaSample.Reactive;

namespace MantaSample.Views;

[View<LocalizationView>]
sealed partial class LocalizationPage_Model : PageModel
{
    public override string Header => LangKey.Page_Localization;

    public LangType[] SupportedLanguages { get; } = [LangType.en, LangType.zh_Hans];

    public LangType CurrentLanguage
    {
        get;
        set => SetField(ref field, value);
    } = Lang.Current;

    public CommandWarper<LangType> ChangeLanguageCommand => GetCommand<LangType>(langType =>
    {
        Lang.Current = langType;
        CurrentLanguage = langType;
    });
}
