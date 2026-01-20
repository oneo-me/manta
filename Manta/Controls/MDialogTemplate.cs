using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public class MDialogTemplate : HeaderedContentControl
{
    static MDialogTemplate()
    {
        FooterProperty.Changed.AddClassHandler<MDialogTemplate>((x, e) => x.OnContentChanged(e));
        CancelButtonProperty.Changed.AddClassHandler<MDialogTemplate>((x, e) => x.OnContentChanged(e));
        ContinueButtonProperty.Changed.AddClassHandler<MDialogTemplate>((x, e) => x.OnContentChanged(e));
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalHeaderAlignmentProperty = AvaloniaProperty.Register<MDialogTemplate, HorizontalAlignment>(
        nameof(HorizontalHeaderAlignment));

    public HorizontalAlignment HorizontalHeaderAlignment
    {
        get => GetValue(HorizontalHeaderAlignmentProperty);
        set => SetValue(HorizontalHeaderAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalHeaderAlignmentProperty = AvaloniaProperty.Register<MDialogTemplate, VerticalAlignment>(
        nameof(VerticalHeaderAlignment));

    public VerticalAlignment VerticalHeaderAlignment
    {
        get => GetValue(VerticalHeaderAlignmentProperty);
        set => SetValue(VerticalHeaderAlignmentProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalFooterAlignmentProperty = AvaloniaProperty.Register<MDialogTemplate, HorizontalAlignment>(
        nameof(HorizontalFooterAlignment));

    public HorizontalAlignment HorizontalFooterAlignment
    {
        get => GetValue(HorizontalFooterAlignmentProperty);
        set => SetValue(HorizontalFooterAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalFooterAlignmentProperty = AvaloniaProperty.Register<MDialogTemplate, VerticalAlignment>(
        nameof(VerticalFooterAlignment));

    public VerticalAlignment VerticalFooterAlignment
    {
        get => GetValue(VerticalFooterAlignmentProperty);
        set => SetValue(VerticalFooterAlignmentProperty, value);
    }

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<MDialogTemplate, object?>(
        nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> FooterTemplateProperty = AvaloniaProperty.Register<MDialogTemplate, IDataTemplate?>(
        nameof(FooterTemplate));

    public IDataTemplate? FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowCancelButtonProperty = AvaloniaProperty.Register<MDialogTemplate, bool>(
        nameof(IsShowCancelButton));

    public bool IsShowCancelButton
    {
        get => GetValue(IsShowCancelButtonProperty);
        set => SetValue(IsShowCancelButtonProperty, value);
    }

    public static readonly StyledProperty<MButtonVariant> CancelButtonVariantProperty = AvaloniaProperty.Register<MDialogTemplate, MButtonVariant>(
        nameof(CancelButtonVariant));

    public MButtonVariant CancelButtonVariant
    {
        get => GetValue(CancelButtonVariantProperty);
        set => SetValue(CancelButtonVariantProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CancelButtonCommandProperty = AvaloniaProperty.Register<MDialogTemplate, ICommand?>(
        nameof(CancelButtonCommand));

    public ICommand? CancelButtonCommand
    {
        get => GetValue(CancelButtonCommandProperty);
        set => SetValue(CancelButtonCommandProperty, value);
    }

    public static readonly StyledProperty<object?> CancelButtonProperty = AvaloniaProperty.Register<MDialogTemplate, object?>(
        nameof(CancelButton));

    public object? CancelButton
    {
        get => GetValue(CancelButtonProperty);
        set => SetValue(CancelButtonProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> CancelButtonTemplateProperty = AvaloniaProperty.Register<MDialogTemplate, IDataTemplate?>(
        nameof(CancelButtonTemplate));

    public IDataTemplate? CancelButtonTemplate
    {
        get => GetValue(CancelButtonTemplateProperty);
        set => SetValue(CancelButtonTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowContinueButtonProperty = AvaloniaProperty.Register<MDialogTemplate, bool>(
        nameof(IsShowContinueButton));

    public bool IsShowContinueButton
    {
        get => GetValue(IsShowContinueButtonProperty);
        set => SetValue(IsShowContinueButtonProperty, value);
    }

    public static readonly StyledProperty<MButtonVariant> ContinueButtonVariantProperty = AvaloniaProperty.Register<MDialogTemplate, MButtonVariant>(
        nameof(ContinueButtonVariant));

    public MButtonVariant ContinueButtonVariant
    {
        get => GetValue(ContinueButtonVariantProperty);
        set => SetValue(ContinueButtonVariantProperty, value);
    }

    public static readonly StyledProperty<ICommand?> ContinueButtonCommandProperty = AvaloniaProperty.Register<MDialogTemplate, ICommand?>(
        nameof(ContinueButtonCommand));

    public ICommand? ContinueButtonCommand
    {
        get => GetValue(ContinueButtonCommandProperty);
        set => SetValue(ContinueButtonCommandProperty, value);
    }

    public static readonly StyledProperty<object?> ContinueButtonProperty = AvaloniaProperty.Register<MDialogTemplate, object?>(
        nameof(ContinueButton));

    public object? ContinueButton
    {
        get => GetValue(ContinueButtonProperty);
        set => SetValue(ContinueButtonProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ContinueButtonTemplateProperty = AvaloniaProperty.Register<MDialogTemplate, IDataTemplate?>(
        nameof(ContinueButtonTemplate));

    public IDataTemplate? ContinueButtonTemplate
    {
        get => GetValue(ContinueButtonTemplateProperty);
        set => SetValue(ContinueButtonTemplateProperty, value);
    }

    public ContentPresenter? FooterPresenter { get; private set; }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var flag = base.RegisterContentPresenter(presenter);

        if (presenter.Name == "PART_FooterPresenter")
        {
            FooterPresenter = presenter;
            flag = true;
        }

        return flag;
    }

    void OnContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
