using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AirKit.Controls.Panels;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls;

public class AutoGrid : PanelExtBase
{
    public static readonly StyledProperty<ColumnDefinitions?> DefinitionsProperty =
        AvaloniaProperty.Register<AutoGrid, ColumnDefinitions?>(nameof(Definitions));

    [SuppressMessage("Usage", "CA2227:集合属性应为只读")]
    public ColumnDefinitions? Definitions
    {
        get => GetValue(DefinitionsProperty);
        set => SetValue(DefinitionsProperty, value);
    }

    static AutoGrid()
    {
        AffectsMeasure<AutoGrid>(DefinitionsProperty);
        AffectsArrange<AutoGrid>(DefinitionsProperty);
    }

    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return AutoGridPanel.Measure(this, children, availableSize);
    }

    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        AutoGridPanel.Arrange(this, children, finalSize);
    }
}
