using Avalonia.Collections;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.ReactiveDemos;

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

        var view = this.GetView();
        var dictionary = this.GetContexts(view);
        foreach (var kvp in dictionary)
            Contexts.Add(kvp);
    }
}
