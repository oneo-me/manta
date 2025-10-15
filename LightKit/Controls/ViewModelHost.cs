using Avalonia;
using Avalonia.Controls;
using LightKit.Reactive.Services;
using LightKit.Reactive.Views;

namespace LightKit.Controls;

public class ViewModelHost : ContentControl
{
    public static readonly StyledProperty<Type?> ViewModelTypeProperty = AvaloniaProperty.Register<ViewModelHost, Type?>(
        nameof(ViewModelType));

    public Type? ViewModelType
    {
        get => GetValue(ViewModelTypeProperty);
        set => SetValue(ViewModelTypeProperty, value);
    }

    public static readonly StyledProperty<ViewModel?> ViewModelProperty = AvaloniaProperty.Register<ViewModelHost, ViewModel?>(
        nameof(ViewModel));

    public ViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ViewModelTypeProperty)
        {
            if (change.NewValue is Type type)
                ViewModel = Service.CreateInstance<ViewModel>(type);
            else
                ViewModel = null;
        }
        else if (change.Property == ViewModelProperty)
        {
            if (change.NewValue is ViewModel viewModel)
                Content = viewModel.CreateView();
            else
                Content = null;
        }
    }
}
