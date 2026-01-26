using System.Collections.ObjectModel;
using Avalonia.Media;
using Manta.Controls.Icons;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<FontView>]
sealed class FontView_Model : DemoViewModel
{
    public static FontView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Text;
    public override string Header => LangKey.Demo_Style_Font;

    public ObservableCollection<FontFamily> Fonts { get; } = [];

    public FontView_Model()
    {
        foreach (var font in FontManager.Current.SystemFonts.OrderBy(f => f.Name))
            Fonts.Add(font);
    }

    public CommandWarper<FontFamily> CopyAsyncCommand => GetCommand<FontFamily>(async font =>
    {
        var clipboard = await this.GetClipboardAsync();
        await clipboard.SetTextAsync(font.Name);
    });
}
