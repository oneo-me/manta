using Avalonia.Markup.Xaml;
using Axe.I18N;
using Axe.I18N.Avalonia;
using LangDemo.Langs;
using Axe.Reactive;
using LangDemo.Views;

namespace LangDemo;

sealed class App : OrphicApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        Lang.Register(LangData.LoadFromAssembly<InnerLangKey>(typeof(App).Assembly));

        mainModel = new Main_Model();
        onExit = null;
    }
}
