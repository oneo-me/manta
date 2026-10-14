using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MantaComponents.ViewModels;

/// <summary>
/// Minimal <see cref="INotifyPropertyChanged"/> base. The sample deliberately avoids the Manta MVVM
/// runtime, so it uses a hand-written observable object instead of a framework base class.
/// </summary>
abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
