using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum MtAlertVariant
{
    Default,
    Destructive
}

[TemplatePart("PART_IconPresenter", typeof(ContentPresenter))]
public class MtAlert : HeaderedContentControl
{
    static MtAlert()
    {
        IconProperty.Changed.AddClassHandler<MtAlert>((x, e) => x.IconChanged(e));
    }

    public static readonly StyledProperty<MtAlertVariant> VariantProperty = AvaloniaProperty.Register<MtAlert, MtAlertVariant>(
        nameof(Variant));

    public MtAlertVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<MtAlert, object?>(nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<MtAlert, IDataTemplate?>(nameof(IconTemplate));

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
