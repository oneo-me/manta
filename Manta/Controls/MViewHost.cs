using Avalonia;
using Avalonia.Controls;
using Manta.Extensions;
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

    public static readonly StyledProperty<object?> DataProperty = AvaloniaProperty.Register<MViewHost, object?>(
        nameof(Data));

    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ViewModelTypeProperty)
        {
            var viewModelType = change.GetNewValue<Type?>();
            ViewModel = viewModelType?.CreateInstance<ViewModel>();
        }
        else if (change.Property == ViewModelProperty)
        {
            var viewModel = change.GetNewValue<ViewModel?>();
            ResetData(viewModel, Data);
            Content = viewModel?.CreateView();
        }
        else if (change.Property == DataProperty)
        {
            var data = change.GetNewValue<object?>();
            ResetData(ViewModel, data);
        }
    }

    static void ResetData(ViewModel? viewModel, object? data)
    {
        if (viewModel is IViewHostTarget viewHostTarget)
            viewHostTarget.SetData(data);
    }
}
