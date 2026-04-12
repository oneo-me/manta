# Platform APIs

## File / Folder Pickers

Manta wraps Avalonia's `StorageProvider` into extension methods on `ViewModel`, handling thread
safety and window acquisition automatically.

```csharp
using Avalonia.Platform.Storage;
```

### OpenFilePickerAsync

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

### OpenFolderPickerAsync

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

### SaveFilePickerAsync

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

### Options Summary

| Method | Options Type | Key Properties |
|--------|-------------|----------------|
| `OpenFilePickerAsync` | `FilePickerOpenOptions` | `Title`, `AllowMultiple`, `FileTypeFilter` |
| `OpenFolderPickerAsync` | `FolderPickerOpenOptions` | `Title`, `AllowMultiple` |
| `SaveFilePickerAsync` | `FilePickerSaveOptions` | `Title`, `SuggestedFileName`, `FileTypeChoices` |

### FilePickerFileType

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

Thread safety and parent-window resolution are handled automatically.

---

## CurrentSystem — Platform Detection

Attached properties for platform-conditional UI in AXAML:

```xml
<!-- Show only on macOS -->
<TextBlock Text="macOS only" IsVisible="{Binding (CurrentSystem.IsMacOS)}" />

<!-- Show only on Windows -->
<Border IsVisible="{Binding (CurrentSystem.IsWindows)}" />

<!-- Show only on Linux -->
<Border IsVisible="{Binding (CurrentSystem.IsLinux)}" />
```

In C#:
```csharp
if (OperatingSystem.IsMacOS()) { /* ... */ }
```

---

## WindowStatus — Observable Window State

Track fullscreen state, button visibility, and platform-specific offsets:

```csharp
protected override void OnViewActivated(CompositeDisposable disposables)
{
    var view = GetView();
    view.GetWindowStatusObservable()
        .Subscribe(status =>
        {
            // status.IsFullScreen — bool
            // status.IsShowButtons — bool (title bar buttons visible)
            // status.Offset — Thickness (macOS: top=38 normal, 0 fullscreen)
        })
        .DisposeWith(disposables);
}
```

**WindowStatus struct:**

| Property | Type | Description |
|----------|------|-------------|
| `IsFullScreen` | `bool` | Window is in fullscreen mode |
| `IsShowButtons` | `bool` | Title bar buttons are visible |
| `Offset` | `Thickness` | Platform-specific insets (macOS traffic lights) |

---

## VisualExtensions — Window Behavior Helpers

### BindTitleBarEvents

Make any element act as a title bar (drag to move, double-tap to maximize):

```csharp
// In code-behind or OnViewActivated
element.BindTitleBarEvents(window, stateControl: true)
    .DisposeWith(disposables);
```

- Double-tap toggles maximize/normal
- Click+drag moves window
- Platform-aware (macOS fullscreen aware)

### EnsureBackground

Prevents hit-test issues on transparent elements:

```csharp
element.EnsureBackground();  // adds transparent background if none exists
```

---

## WindowExtensions

```csharp
// Activate and bring to front
window.FocusActivate();

// Center window in owner or screen
window.Centered();

// Auto-center on programmatic resize
window.KeepCentered();
```

---

## ImageLoadService — Async Image Cache

Registered by default in MApp. Supports web, local, and Avalonia resource images with disk caching.

### Setup

```csharp
// In App.axaml.cs OnInitialize
var imageService = Service.GetService<ImageLoadService>();
imageService.CacheDirectory = Path.Combine(cacheFolder, "ImageCache");
```

### Image Sources

| Source Type | Constructor | Description |
|-------------|------------|-------------|
| `WebImageSource` | `new(Uri uri)` | HTTP/HTTPS with disk caching |
| `LocalImageSource` | `new(string path)` | Local file system |
| `AvaloniaImageSource` | `new(Uri uri, Uri? baseUri)` | `avares://` protocol |
| `BitmapImageSource` | `new(Bitmap bitmap)` | In-memory bitmap |

### Usage in Code

```csharp
var imageService = Service.GetService<ImageLoadService>();
var bitmap = await imageService.LoadAsync(
    new WebImageSource(new Uri("https://example.com/photo.jpg")),
    CancellationToken.None
);
```

### Usage in AXAML

`MImage` control auto-resolves string sources:

```xml
<MImage Source="https://example.com/photo.jpg" Width="200" />
<MImage Source="avares://MyApp/Assets/logo.png" Width="100" />
```

String auto-conversion:
- `https://...` → `WebImageSource`
- `avares://...` → `AvaloniaImageSource`
- File path → `LocalImageSource`

### Caching

- Uses XxHash3 of the URL/path as cache filename
- Thread-safe with `SemaphoreSlim[32]` lock array
- Falls back to re-download on cache miss/corruption
