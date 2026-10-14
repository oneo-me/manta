namespace MantaPrepare;

sealed class Options(string appDir)
{
    public string AppDir { get; } = appDir;

    public string SolutionDir { get; set; } = string.Empty;
    public string ProjectDir { get; set; } = string.Empty;
    public string ControlsDir { get; set; } = string.Empty;
    public string DesignControlsDir { get; set; } = string.Empty;
    public string BaseControlsDir { get; set; } = string.Empty;
    public string SupportedControlsDir { get; set; } = string.Empty;
    public string UnSupportedControlsDir { get; set; } = string.Empty;
    public string MantaThemeFile { get; set; } = string.Empty;
    public string LightThemeFile { get; set; } = string.Empty;
    public string DarkThemeFile { get; set; } = string.Empty;
    public string AutoDemosDir { get; set; } = string.Empty;
}
