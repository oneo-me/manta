using System.ComponentModel;
using System.Runtime.CompilerServices;
using AirKit.Reactive.Logging;

namespace AirKit.Reactive;

public abstract class Model : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    protected bool SetValue<T>(ref T backingField, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            return false;

        backingField = newValue;

        Log.Verbose("Model", "{propertyName} changed to {newValue}", propertyName, newValue);

        OnPropertyChanged(propertyName);

        return true;
    }
}
