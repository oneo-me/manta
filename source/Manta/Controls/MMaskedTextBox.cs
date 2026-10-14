using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;

namespace Manta.Controls;

[TemplatePart(nameof(PART_Caret), typeof(Border))]
[TemplatePart(nameof(PART_TextPresenter), typeof(TextPresenter))]
public class MMaskedTextBox : MaskedTextBox
{
    protected override Type StyleKeyOverride { get; } = typeof(MTextBox);

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
