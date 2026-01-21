using Manta.Markdown.Controls;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<MarkdownView>]
sealed class MarkdownView_Model : DemoViewModel
{
    public static MarkdownView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MMarkdown);
}
