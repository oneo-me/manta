using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum MRadioButtonVariant
{
    Default,
    Bordered
}

public class MRadioButton : RadioButton
{
    static MRadioButton()
    {
        HeaderProperty.Changed.AddClassHandler<MRadioButton>((x, e) => x.HeaderChanged(e));
    }

    public static readonly StyledProperty<MRadioButtonVariant> VariantProperty = AvaloniaProperty.Register<MRadioButton, MRadioButtonVariant>(
        nameof(Variant));

    public MRadioButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderProperty = AvaloniaProperty.Register<MRadioButton, object?>(nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty = AvaloniaProperty.Register<MRadioButton, IDataTemplate?>(nameof(HeaderTemplate));

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
