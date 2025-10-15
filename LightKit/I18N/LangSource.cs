using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using LightKit.I18N.Core;

namespace LightKit.I18N;

sealed class LangSource : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public LangType Current
    {
        get;
        set
        {
            if (SetField(ref field, value) is false)
                return;

            var cultureInfo = value.ToCultureInfo();
            if (cultureInfo is null)
                return;

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
