using Manta.Dialogs;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;
using MAlert = Manta.Dialogs.MAlert;

namespace MantaDemo.Demos.ReactiveDemos;

[View<DialogView>]
sealed partial class DialogView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_Dialog;

    public CommandWarper AlertAsyncCommand => GetCommand(async () =>
    {
        await MAlert.ShowDialogAsync(this, new("This is an alert dialog.")).ConfigureAwait(true);
    });

    public CommandWarper ConfirmAsyncCommand => GetCommand(async () =>
    {
        var result = await MConfirm.ShowDialogAsync(this, new("This is an alert dialog.")).ConfigureAwait(true);
        if (result)
            await MAlert.ShowDialogAsync(this, new("You clicked OK.")).ConfigureAwait(true);
    });

    public bool AllowMultiple
    {
        get;
        set => SetValue(ref field, value);
    } = true;

    public CommandWarper OpenFileAsyncCommand => GetCommand(async () =>
    {
        var result = await this.OpenFilePickerAsync(new()
        {
            Title = "Open a file",
            AllowMultiple = AllowMultiple,
            FileTypeFilter =
            [
                new("Image Files")
                {
                    Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif"],
                    MimeTypes = ["image/png", "image/jpeg", "image/bmp", "image/gif"]
                },
                new("All Files")
                {
                    Patterns = ["*.*"],
                    MimeTypes = ["*/*"]
                }
            ]
        }).ConfigureAwait(true);

        if (result.Count == 0)
            return;

        await MAlert.ShowDialogAsync(this, new($"You selected file:\n{string.Join("\n", result.Select(f => f.Name))}")).ConfigureAwait(true);
    });

    public CommandWarper OpenFolderAsyncCommand => GetCommand(async () =>
    {
        var result = await this.OpenFolderPickerAsync(new()
        {
            Title = "Open a folder",
            AllowMultiple = AllowMultiple
        }).ConfigureAwait(true);

        if (result.Count == 0)
            return;

        await MAlert.ShowDialogAsync(this, new($"You selected folder:\n{string.Join("\n", result.Select(f => f.Name))}")).ConfigureAwait(true);
    });

    public CommandWarper SaveFileAsyncCommand => GetCommand(async () =>
    {
        var result = await this.SaveFilePickerAsync(new()
        {
            Title = "Save a file",
            SuggestedFileName = "NewFile.txt",
            FileTypeChoices =
            [
                new("Text Files")
                {
                    Patterns = ["*.txt"],
                    MimeTypes = ["text/plain"]
                },
                new("All Files")
                {
                    Patterns = ["*.*"],
                    MimeTypes = ["*/*"]
                }
            ]
        }).ConfigureAwait(true);

        if (result is null)
            return;

        await MAlert.ShowDialogAsync(this, new($"You selected file to save:\n{result.Name}")).ConfigureAwait(true);
    });
}
