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
        await AlertDialog.ShowDialogAsync(this, new("This is an alert dialog."));
    }

    public async Task ConfirmAsync()
    {
        var result = await ConfirmDialog.ShowDialogAsync(this, new("This is an alert dialog."));
        if (result)
            await AlertDialog.ShowDialogAsync(this, new("You clicked OK."));
    }

    public bool AllowMultiple
    {
        get;
        set => SetValue(ref field, value);
    } = true;

    public async Task OpenFileAsync()
    {
        var storageProvider = await this.GetStorageProviderAsync();
        var result = await storageProvider.OpenFilePickerAsync(new()
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
        });

        if (result.Count == 0)
            return;

        await AlertDialog.ShowDialogAsync(this, new($"You selected file:\n{string.Join("\n", result.Select(f => f.Name))}"));
    }

    public async Task OpenFolderAsync()
    {
        var storageProvider = await this.GetStorageProviderAsync();
        var result = await storageProvider.OpenFolderPickerAsync(new()
        {
            Title = "Open a folder",
            AllowMultiple = AllowMultiple
        });

        if (result.Count == 0)
            return;

        await AlertDialog.ShowDialogAsync(this, new($"You selected folder:\n{string.Join("\n", result.Select(f => f.Name))}"));
    }

    public async Task SaveFileAsync()
    {
        var storageProvider = await this.GetStorageProviderAsync();
        var result = await storageProvider.SaveFilePickerAsync(new()
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
        });

        if (result is null)
            return;

        await AlertDialog.ShowDialogAsync(this, new($"You selected file to save:\n{result.Name}"));
    }
}
