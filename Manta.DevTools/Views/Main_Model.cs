using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<Main>]
sealed class Main_Model : ViewModel
{
    public static Main_Model Design => new();

    public bool IsVisible
    {
        get;
        set => SetValue(ref field, value);
    } = true;
}
