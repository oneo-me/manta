using Axe.Reactive;
using AxeDemo.Controls;
using AxeDemo.Langs;
using AxeDemo.Reactive;
using AxeDemo.Views;

namespace AxeDemo.Demos;

[View<ReactiveView>]
sealed class ReactiveView_Model : HeaderRegionModel
{
    public static ReactiveView_Model Design => new();

    public override IconKey Icon => IconKey.Reactive;
    public override LangKey Header => LangKey.Reactive;

    public string Folder => DesktopApp.Folder;
    public string File => DesktopApp.File;

    public string Input
    {
        get;
        set => SetValue(ref field, value);
    } = "Empty";

    public void OpenNewWindow()
    {
        var mainModel = new Main_Model();
        var window = mainModel.CreateWindow();
        var owner = this.GetWindow();
        window.Width = owner.Width - 50;
        window.Height = owner.Height - 50;
        window.Show(owner);
    }
}
