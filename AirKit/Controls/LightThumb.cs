using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Metadata;

namespace AirKit.Controls;

[TemplatePart("PART_ContentPresenter", typeof(ContentPresenter))]
public class LightThumb : Thumb
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<LightThumb, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    public static readonly StyledProperty<object?> ContentProperty = AvaloniaProperty.Register<LightThumb, object?>(
        nameof(Content));

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty = AvaloniaProperty.Register<LightThumb, IDataTemplate?>(
        nameof(ContentTemplate));

    public IDataTemplate? ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentProperty)
            ContentChanged(change);
    }

    void ContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
