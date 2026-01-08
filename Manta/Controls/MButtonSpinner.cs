using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Controls;

public class MButtonSpinner : ButtonSpinner
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MButtonSpinner, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    Button? PART_IncreaseButton;
    Button? PART_DecreaseButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_IncreaseButton = e.NameScope.Find<Button>("PART_IncreaseButton");
        PART_DecreaseButton = e.NameScope.Find<Button>("PART_DecreaseButton");

        UpdateButtonCornerRadius();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ButtonSpinnerLocationProperty || change.Property == CornerRadiusProperty)
            UpdateButtonCornerRadius();
    }

    void UpdateButtonCornerRadius()
    {
        // Rounded corners can't be handled perfectly, but they are still good in normal scenarios.

        if (PART_IncreaseButton is not null)
        {
            var offset = BorderThickness.Left / 2d;

            if (ButtonSpinnerLocation == Location.Left)
                PART_IncreaseButton.CornerRadius = new(
                    CornerRadius.TopRight - offset,
                    0,
                    0,
                    0);
            else
                PART_IncreaseButton.CornerRadius = new(
                    0,
                    CornerRadius.TopRight - offset,
                    0,
                    0);
        }

        if (PART_DecreaseButton is not null)
        {
            var offset = BorderThickness.Right / 2d;

            if (ButtonSpinnerLocation == Location.Left)
                PART_DecreaseButton.CornerRadius = new(
                    0,
                    0,
                    0,
                    CornerRadius.BottomRight - offset);
            else
                PART_DecreaseButton.CornerRadius = new(
                    0,
                    0,
                    CornerRadius.BottomRight - offset,
                    0);
        }
    }
}
