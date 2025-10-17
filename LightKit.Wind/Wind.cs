using Avalonia;
using Avalonia.Controls;

namespace LightKit.Wind;

public static class Wind
{
    public enum ButtonType
    {
        None,

        IconButton
    }

    public static readonly AttachedProperty<ButtonType> ButtonProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, ButtonType>("Button");

    public static void SetButton(Control obj, ButtonType value)
    {
        obj.SetValue(ButtonProperty, value);
    }

    public static ButtonType GetButton(Control obj)
    {
        return obj.GetValue(ButtonProperty);
    }

    public enum ListBoxType
    {
        None,

        Sidebar
    }

    public static readonly AttachedProperty<ListBoxType> ListBoxProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, ListBoxType>("ListBox");

    public static void SetListBox(Control obj, ListBoxType value)
    {
        obj.SetValue(ListBoxProperty, value);
    }

    public static ListBoxType GetListBox(Control obj)
    {
        return obj.GetValue(ListBoxProperty);
    }

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
