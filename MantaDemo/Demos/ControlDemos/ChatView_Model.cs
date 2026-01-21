using Manta.Chat.Controls;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<ChatView>]
sealed class ChatView_Model : DemoViewModel
{
    public static ChatView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MChat);
}
