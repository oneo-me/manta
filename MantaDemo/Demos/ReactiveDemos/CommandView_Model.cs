using Avalonia.Input;
using Manta.Controls;
using Manta.Reactive.Commands;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

sealed class CommandView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new CommandView();
    }

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Command;

    public int ClickCount
    {
        get;
        set => SetValue(ref field, value);
    }

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
        ClickCount++;
    }, () => CanExecuted);

    public CommandWarper ExecuteAsyncCommand => GetCommand(async () =>
    {
        await Task.Delay(3000);
        ClickCount++;
    }, () => CanExecuted);

    public string DropFile
    {
        get;
        set => SetValue(ref field, value);
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
        set => SetValue(ref field, value);
    }

    public CommandWarper DoubleTappedCommand => GetCommand(() =>
    {
        DoubleClickCount++;
    });
}
