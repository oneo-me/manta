using System.Windows.Input;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Dialogs;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.ReactiveDemos;

[View<DialogView>]
sealed class DialogView_Model : DemoViewModel
{
    public static DialogView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Dialog;

    public ICommand AlertAsyncCommand { get; }
    public ICommand ConfirmAsyncCommand { get; }

    public DialogView_Model()
    {
        AlertAsyncCommand = new CommandWarper(AlertAsync);
        ConfirmAsyncCommand = new CommandWarper(ConfirmAsync);
    }

    public async Task AlertAsync()
    {
        var window = this.GetWindow();
        await AlertDialog.ShowDialogAsync(window, new("This is an alert dialog."));
    }

    public async Task ConfirmAsync()
    {
        var window = this.GetWindow();
        var result = await ConfirmDialog.ShowDialogAsync(window, new("This is an alert dialog."));
        if (result)
            await AlertDialog.ShowDialogAsync(window, new("You clicked OK."));
    }
}
