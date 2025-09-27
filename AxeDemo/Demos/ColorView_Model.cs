using Axe.Reactive;

namespace AxeDemo.Demos;

[View<ColorView>]
sealed class ColorView_Model : ViewModel
{
    public static ColorView_Model Design => new();
}
