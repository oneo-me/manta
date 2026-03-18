# Platform APIs (File/Folder Pickers)

Manta wraps Avalonia's `StorageProvider` into extension methods on `ViewModel`, handling thread
safety and window acquisition automatically.

---

## File Picker Extension Methods

All three methods live in `Manta.Reactive.Views` and are called directly on `this` inside any
`ViewModel` or `WindowModel`:

```csharp
using Avalonia.Platform.Storage;
```

### `OpenFilePickerAsync`

```csharp
Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
```

```csharp
var files = await this.OpenFilePickerAsync(new()
{
    Title = "Open File",
    AllowMultiple = false,
    FileTypeFilter =
    [
        new("Image Files") { Patterns = ["*.png", "*.jpg"], MimeTypes = ["image/png", "image/jpeg"] },
        new("All Files")   { Patterns = ["*.*"],            MimeTypes = ["*/*"] }
    ]
});

if (files.Count > 0)
{
    await using var stream = await files[0].OpenReadAsync();
    // ...
}
```

### `OpenFolderPickerAsync`

```csharp
Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
```

```csharp
var folders = await this.OpenFolderPickerAsync(new()
{
    Title = "Select Folder",
    AllowMultiple = false
});

if (folders.Count > 0)
{
    var folder = folders[0];
    var file = await folder.CreateFileAsync("output.txt");
}
```

### `SaveFilePickerAsync`

```csharp
Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
```

Returns `null` if the user cancels.

```csharp
var file = await this.SaveFilePickerAsync(new()
{
    Title = "Save File",
    SuggestedFileName = "export.txt",
    FileTypeChoices =
    [
        new("Text Files") { Patterns = ["*.txt"], MimeTypes = ["text/plain"] },
        new("All Files")  { Patterns = ["*.*"],   MimeTypes = ["*/*"] }
    ]
});

if (file is not null)
{
    await using var stream = await file.OpenWriteAsync();
    // ...
}
```

---

## Options Summary

| Method | Options Type | Key Properties |
|--------|-------------|----------------|
| `OpenFilePickerAsync` | `FilePickerOpenOptions` | `Title`, `AllowMultiple`, `FileTypeFilter` |
| `OpenFolderPickerAsync` | `FolderPickerOpenOptions` | `Title`, `AllowMultiple` |
| `SaveFilePickerAsync` | `FilePickerSaveOptions` | `Title`, `SuggestedFileName`, `FileTypeChoices` |

---

## FilePickerFileType

```csharp
new("Display Name")
{
    Patterns  = ["*.png", "*.jpg"],         // glob patterns
    MimeTypes = ["image/png", "image/jpeg"] // MIME types (used on some platforms)
}
```

---

## Clipboard

```csharp
public async Task GetClipboardAsync()
```

Returns the `IClipboard` for the ViewModel's window. Thread-safe (auto-invokes on UI thread).

```csharp
public CommandWarper CopyCommand => GetCommand(async () =>
{
    var clipboard = await this.GetClipboardAsync();
    await clipboard.SetTextAsync(SelectedText);
});

public CommandWarper PasteCommand => GetCommand(async () =>
{
    var clipboard = await this.GetClipboardAsync();
    var text = await clipboard.GetTextAsync();
    if (text is not null) InsertText(text);
});
```

---

## ViewModel Usage Pattern

```csharp
public CommandWarper BrowseCommand => GetCommand(async () =>
{
    var folders = await this.OpenFolderPickerAsync(new() { Title = "Select Output Folder" });
    if (folders.Count > 0)
        OutputPath = folders[0].Path.LocalPath;
});
```

Thread safety and parent-window resolution are handled automatically by Manta — no extra code needed.
