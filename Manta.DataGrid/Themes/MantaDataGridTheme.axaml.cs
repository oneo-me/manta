using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.DataGrid.Themes;

public sealed class MantaDataGridTheme : Styles
{
    public MantaDataGridTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
