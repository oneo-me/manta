using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<ObjectPropertyDialog>]
sealed class ObjectPropertyDialog_Model : DialogModel
{
    public string PropertyName { get; set; } = string.Empty;
    public object? PropertyValue { get; set; }

    public string Header => $"{PropertyName}: {PropertyValue?.GetType().Name ?? "null"}";

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.Width = 400;
        window.Height = 500;
    }
}
