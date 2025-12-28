using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum LightCheckBoxVariant
{
    Default,
    Bordered
}

public class LightCheckBox : CheckBox
{
    static LightCheckBox()
    {
        HeaderProperty.Changed.AddClassHandler<LightCheckBox>((x, e) => x.HeaderChanged(e));
    }

    public static readonly StyledProperty<LightCheckBoxVariant> VariantProperty = AvaloniaProperty.Register<LightCheckBox, LightCheckBoxVariant>(
        nameof(Variant));

    public LightCheckBoxVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderProperty = AvaloniaProperty.Register<LightCheckBox, object?>(nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty = AvaloniaProperty.Register<LightCheckBox, IDataTemplate?>(nameof(HeaderTemplate));

    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public ContentPresenter? HeaderPresenter { get; private set; }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var flag = base.RegisterContentPresenter(presenter);

        if (presenter.Name == "PART_HeaderPresenter")
        {
            HeaderPresenter = presenter;
            flag = true;
        }

        return flag;
    }

    void HeaderChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
