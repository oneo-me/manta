using Avalonia.Input;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<CommandView>]
sealed partial class CommandView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Command;

    public int ClickCount
    {
        get;
        set => SetField(ref field, value);
    }

    public bool CanExecuted
    {
        get;
        set
        {
            if (SetField(ref field, value))
            {
                ExecuteCommand.OnChanged();
                ExecuteAsyncCommand.OnChanged();
            }
        }
    }

    public CommandWarper ExecuteCommand => GetCommand(() =>
    {
        ClickCount++;
    }, () => CanExecuted);

    public CommandWarper ExecuteAsyncCommand => GetCommand(async () =>
    {
        await Task.Delay(3000).ConfigureAwait(true);
        ClickCount++;
    }, () => CanExecuted);

    public string DropFile
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public CommandWarper<DragEventArgs> DragDropCommand => GetCommand<DragEventArgs>(e =>
    {
        // Asynchronous calls are not supported
        var tryGetFile = e.DataTransfer.TryGetFile();
        if (tryGetFile is not null)
            DropFile = tryGetFile.Path.LocalPath;
    });

    public int DoubleClickCount
    {
        get;
        set => SetField(ref field, value);
    }

    public CommandWarper DoubleTappedCommand => GetCommand(() =>
    {
        DoubleClickCount++;
    });
}
