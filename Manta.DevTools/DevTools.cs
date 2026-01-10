using Avalonia.Controls;
using Manta.Controls;

namespace Manta.DevTools;

sealed class DevTools
{
    readonly DevToolsOptions _options;

    public DevTools(DevToolsOptions options)
    {
        _options = options;

        if (MWindow.PrepareAttach is not null)
            return;

        MWindow.PrepareAttach = OnPrepareAttach;
    }

    void OnPrepareAttach(MWindow window, Grid windowContent)
    {
        _ = new DevToolsAttacher(_options, window, windowContent);
    }
}
