using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Manta.Controls;

public enum MTextBoxVariant
{
    Default,
    NoStyle
}

[TemplatePart(nameof(PART_Caret), typeof(Border))]
[TemplatePart(nameof(PART_TextPresenter), typeof(TextPresenter))]
public class MTextBox : TextBox
{
    public static KeyGesture? SelectAllGesture => Application.Current?.PlatformSettings?.HotkeyConfiguration.SelectAll.FirstOrDefault();

    public static readonly StyledProperty<MTextBoxVariant> VariantProperty = AvaloniaProperty.Register<MTextBox, MTextBoxVariant>(
        nameof(Variant));

    public MTextBoxVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<bool> IsPasswordBoxProperty = AvaloniaProperty.Register<MTextBox, bool>(
        nameof(IsPasswordBox));

    public bool IsPasswordBox
    {
        get => GetValue(IsPasswordBoxProperty);
        set => SetValue(IsPasswordBoxProperty, value);
    }

    Border? PART_Caret;
    TextPresenter? PART_TextPresenter;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Caret = e.NameScope.Get<Border>(nameof(PART_Caret));
        PART_TextPresenter = e.NameScope.Get<TextPresenter>(nameof(PART_TextPresenter));

        _ = new MTextBoxCaret(this, PART_Caret, PART_TextPresenter);
    }
}
