using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum LightAlertVariant
{
    Default,
    Destructive
}

[TemplatePart("PART_IconPresenter", typeof(ContentPresenter))]
public class LightAlert : HeaderedContentControl
{
    static LightAlert()
    {
        IconProperty.Changed.AddClassHandler<LightAlert>((x, e) => x.IconChanged(e));
    }

    public static readonly StyledProperty<LightAlertVariant> VariantProperty = AvaloniaProperty.Register<LightAlert, LightAlertVariant>(
        nameof(Variant));

    public LightAlertVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<LightAlert, object?>(nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<LightAlert, IDataTemplate?>(nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public ContentPresenter? IconPresenter { get; private set; }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var flag = base.RegisterContentPresenter(presenter);

        if (presenter.Name == "PART_IconPresenter")
        {
            IconPresenter = presenter;
            flag = true;
        }

        return flag;
    }

    void IconChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
