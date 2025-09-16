using ControlsDemo.Extensions;
using Axe.Logging;
using ControlsDemo.Configs;

namespace ControlsDemo.Services;

sealed class ConfigService
{
    readonly string _configFile;

    public AppConfig Config { get; }

    public ConfigService(string configFile)
    {
        _configFile = configFile;

        Log.Information("Loading config from {ConfigFile}", _configFile);

        if (File.Exists(_configFile) is false)
        {
            Config = new();
            return;
        }

        var yaml = File.ReadAllText(_configFile);
        Config = yaml.DeserializeYaml<AppConfig>();
    }

    public void Save()
    {
        Log.Information("Saving config to {ConfigFile}", _configFile);

        var configDirectory = Path.GetDirectoryName(_configFile);
        if (configDirectory is null)
            throw new InvalidOperationException("Config file directory is null");
        Directory.CreateDirectory(configDirectory);
        var yaml = Config.SerializeYaml();
        File.WriteAllText(_configFile, yaml);
    }
}
