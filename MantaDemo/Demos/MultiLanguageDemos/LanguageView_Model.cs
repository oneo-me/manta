using Manta.Globalization;
using Manta.Globalization.Core;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.MultiLanguageDemos;

[View<LanguageView>]
sealed partial class LanguageView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.MultiLanguage;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_MultiLanguage_Language;

    public LangType[] SupportedLanguages { get; } = [LangType.en, LangType.zh_Hans, LangType.ja];

    public LangType SelectedLanguage
    {
        get;
        set
        {
            if (SetField(ref field, value))
            {
                Lang.Current = value;
                UpdatedAt = DateTimeOffset.Now;
                RefreshSnapshot();
            }
        }
    } = Lang.Current;

    public string UserName
    {
        get;
        set
        {
            if (SetField(ref field, value))
                RefreshSnapshot();
        }
    } = "Manta";

    public DateTimeOffset UpdatedAt
    {
        get;
        set => SetField(ref field, value);
    } = DateTimeOffset.Now;

    public string SnapshotGreeting
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public string CurrentLanguageCode
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public LanguageView_Model()
    {
        RefreshSnapshot();
    }

    public CommandWarper<LangType> ChangeLanguageCommand => GetCommand<LangType>(lang => SelectedLanguage = lang);

    public CommandWarper CycleLanguageCommand => GetCommand(() =>
    {
        var index = Array.IndexOf(SupportedLanguages, SelectedLanguage);
        var nextIndex = (index + 1) % SupportedLanguages.Length;
        SelectedLanguage = SupportedLanguages[nextIndex];
    });

    void RefreshSnapshot()
    {
        CurrentLanguageCode = SelectedLanguage.GetMetadata().SourceType;
        SnapshotGreeting = Lang.Get(new(LangKey.Demo_MultiLanguage_Language_Greeting, UserName)
        {
            LangType = SelectedLanguage
        });
    }
}
