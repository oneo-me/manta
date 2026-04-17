using Avalonia;
using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.Controls;

public class MViewHost : ContentControl
{
    public static readonly DirectProperty<MViewHost, ViewModel?> ViewModelProperty = AvaloniaProperty.RegisterDirect<MViewHost, ViewModel?>(
        nameof(ViewModel), o => o.ViewModel, (o, v) => o.ViewModel = v);

    public ViewModel? ViewModel
    {
        get;
        set => SetAndRaise(ViewModelProperty, ref field, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ViewModelProperty)
            Content = change.GetNewValue<ViewModel?>()?.BuildView();
    }
}
