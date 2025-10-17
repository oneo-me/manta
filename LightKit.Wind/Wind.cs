using Avalonia;
using Avalonia.Controls;

namespace LightKit.Wind;

public static class Wind
{
    public enum TabStripType
    {
        None,

        RootTab
    }

    public static readonly AttachedProperty<TabStripType> TabStripProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, TabStripType>("TabStrip");

    public static void SetTabStrip(Control obj, TabStripType value)
    {
        obj.SetValue(TabStripProperty, value);
    }

    public static TabStripType GetTabStrip(Control obj)
    {
        return obj.GetValue(TabStripProperty);
    }
}
