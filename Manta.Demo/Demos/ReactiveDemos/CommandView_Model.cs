using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.ReactiveDemos;

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

    public CommandWarper ExecuteCommand { get; }
    public CommandWarper ExecuteAsyncCommand { get; }

    public CommandView_Model()
    {
        ExecuteCommand = new(Execute, () => CanExecuted);
        ExecuteAsyncCommand = new(ExecuteAsync, () => CanExecuted);
    }

    public void Execute()
    {
    }

    public bool IsExecuting
    {
        get;
        private set => SetValue(ref field, value);
    }

    public async Task ExecuteAsync()
    {
        IsExecuting = true;
        await Task.Delay(3000);
        IsExecuting = false;
    }
}
