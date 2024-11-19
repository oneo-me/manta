using AirKit.Reactive.Demo.Configs;
using AirKit.Reactive.Demo.Extensions;
using AirKit.Reactive.Logging;
using AirKit.Reactive.Paths;

namespace AirKit.Reactive.Demo.Services;

sealed class ConfigService
{
    readonly FileInfo _configFile;

    public AppConfig Config { get; }

    public ConfigService(FileInfo configFile)
    {
        _configFile = configFile;

        Log.Information("ConfigService", "Loading config from {ConfigFile}", _configFile.FullName);

        if (_configFile.Exists is false)
        {
            Config = new();
            return;
        }

        var yaml = _configFile.ReadText();
        Config = yaml.DeserializeYaml<AppConfig>();
    }

    public void Save()
    {
        Log.Information("ConfigService", "Saving config to {ConfigFile}", _configFile.FullName);

        var configDirectory = _configFile.Directory;
        if (configDirectory is null)
            throw new InvalidOperationException("Config file directory is null");
        configDirectory.Create();
        var yaml = Config.SerializeYaml();
        _configFile.WriteText(yaml);
    }
}
