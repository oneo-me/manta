using Avalonia.Controls;
using Manta.Globalization;
using Manta.Langs;

namespace Manta;

static class MAppMenus
{
    public static void RegisterDefaultMenus(MApp app)
    {
        if (Design.IsDesignMode is false && OperatingSystem.IsMacOS())
        {
            var menu = NativeMenu.GetMenu(app);
            if (menu is null)
                return;

            // Remove default menu items (About, Services)
            menu.Items.RemoveAt(0);
            menu.Items.RemoveAt(0);
            menu.Items.RemoveAt(0);
            menu.Items.RemoveAt(0);

            foreach (var item in menu.Items.OfType<NativeMenuItem>())
                if (item.Header?.StartsWith("Hide") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Hide_App, app.Name));
                else if (item.Header?.StartsWith("Hide Other") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Hide_Others));
                else if (item.Header?.StartsWith("Show All") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Show_All));
                else if (item.Header?.StartsWith("Quit") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Quit, app.Name));
        }
    }
}
