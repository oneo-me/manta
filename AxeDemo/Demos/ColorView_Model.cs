using Axe.Reactive.Views;

namespace AxeDemo.Demos;

[View<ColorView>]
sealed class ColorView_Model : ViewModel
{
    public static ColorView_Model Design => new();
}
