using MantaDemo.Models;

namespace MantaDemo.Services;

sealed class ConfigService
{
    readonly string _configFile;

    public Config Config { get; }

    public ConfigService(string configFile)
    {
        _configFile = configFile;

        Config ??= new();
    }

    public void Save()
    {
    }
}
