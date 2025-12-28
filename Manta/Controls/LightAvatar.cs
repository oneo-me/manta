using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public class LightAvatar : ContentControl
{
    static LightAvatar()
    {
        FallbackContentProperty.Changed.AddClassHandler<LightAvatar>((x, e) => x.FallbackContentChanged(e));
    }

    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<LightAvatar, double>(
        nameof(Size));

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<object?> FallbackContentProperty = AvaloniaProperty.Register<LightAvatar, object?>(nameof(FallbackContent));

    public object? FallbackContent
    {
        get => GetValue(FallbackContentProperty);
        set => SetValue(FallbackContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> FallbackContentTemplateProperty = AvaloniaProperty.Register<LightAvatar, IDataTemplate?>(nameof(FallbackContentTemplate));

    public IDataTemplate? FallbackContentTemplate
    {
        get => GetValue(FallbackContentTemplateProperty);
        set => SetValue(FallbackContentTemplateProperty, value);
    }

    public ContentPresenter? FallbackContentPresenter { get; private set; }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var flag = base.RegisterContentPresenter(presenter);

        if (presenter.Name == "PART_FallbackContentPresenter")
        {
            FallbackContentPresenter = presenter;
            flag = true;
        }

        return flag;
    }

    void FallbackContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
