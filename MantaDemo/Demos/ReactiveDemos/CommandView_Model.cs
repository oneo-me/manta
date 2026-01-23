using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<CommandView>]
sealed class CommandView_Model : DemoViewModel
{
    public static CommandView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Command;

    public bool CanExecuted
    {
        get;
        set
        {
            if (SetValue(ref field, value))
            {
                ExecuteCommand.OnChanged();
                ExecuteAsyncCommand.OnChanged();
            }
        }
    }

    public CommandWarper ExecuteCommand => GetCommand(() =>
    {
    });

    public CommandWarper ExecuteAsyncCommand => GetCommand(() => Task.Delay(3000));
}
