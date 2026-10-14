using Avalonia.Markup.Xaml;
using Manta;
using Manta.Globalization;
using MantaSample.Views;
using LangData = MantaSample.Langs.LangData;

namespace MantaSample;

sealed class App : MApp
{
    public App()
    {
        Name = nameof(MantaSample);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out MAppOptions options)
    {
        Lang.Register(LangData.Load());

        options = new()
        {
            MainModel = () => new Main_Model()
        };
    }
}
