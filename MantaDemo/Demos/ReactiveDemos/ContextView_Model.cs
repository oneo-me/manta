using Avalonia.Collections;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ContextView>]
sealed class ContextView_Model : DemoViewModel
{
    public static ContextView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_ViewContext;

    public AvaloniaList<KeyValuePair<string, object?>> Contexts { get; } = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var dictionary = this.GetContexts();
        foreach (var kvp in dictionary)
            Contexts.Add(kvp);
    }
}
