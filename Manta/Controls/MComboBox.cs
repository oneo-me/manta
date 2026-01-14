using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Manta.Controls;

public class MComboBox : ComboBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MComboBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MComboBoxItem>(item, out recycleKey);
    }

    TextBox? PART_EditableTextBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Synchronize focus style with TextBox

        PART_EditableTextBox = e.NameScope.Get<TextBox>(nameof(PART_EditableTextBox));
        PART_EditableTextBox.GotFocus += TextBoxOnGotFocus;
        PART_EditableTextBox.LostFocus += TextBoxOnLostFocus;
    }

    void TextBoxOnGotFocus(object? sender, GotFocusEventArgs gotFocusEventArgs)
    {
        PseudoClasses.Set(":focus", true);
    }

    void TextBoxOnLostFocus(object? sender, RoutedEventArgs routedEventArgs)
    {
        PseudoClasses.Set(":focus", false);
    }
}

public class MComboBoxItem : ComboBoxItem;
