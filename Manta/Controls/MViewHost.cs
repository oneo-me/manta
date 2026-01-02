using Avalonia;
using Avalonia.Controls;
using Manta.Reactive.Services;
using Manta.Reactive.Views;

namespace Manta.Controls;

public class MViewHost : ContentControl
{
    public static readonly StyledProperty<Type?> ViewModelTypeProperty = AvaloniaProperty.Register<MViewHost, Type?>(
        nameof(ViewModelType));

    public Type? ViewModelType
    {
        get => GetValue(ViewModelTypeProperty);
        set => SetValue(ViewModelTypeProperty, value);
    }

    public static readonly StyledProperty<ViewModel?> ViewModelProperty = AvaloniaProperty.Register<MViewHost, ViewModel?>(
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
            var viewModelType = change.GetNewValue<Type?>();
            ViewModel = viewModelType is null ? null : Service.CreateInstance<ViewModel>(viewModelType);
        }
        else if (change.Property == ViewModelProperty)
        {
            Content = change.GetNewValue<ViewModel?>()?.CreateView();
        }
    }
}
