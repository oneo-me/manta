using Manta.Logging;
using MantaDemo.Helpers;
using MantaDemo.Models;

namespace MantaDemo.Services;

sealed class ConfigService
{
    readonly string _configFile;

    public Config Config { get; }

    public ConfigService(string configFile)
    {
        _configFile = configFile;

        try
        {
            if (File.Exists(_configFile))
            {
                using var reader = new StreamReader(_configFile);
                Config = ConfigYamlStaticContext.Deserializer.Deserialize<Config>(reader);
            }
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load config file, using default config. Error: {ErrorMessage}", e.Message);
        }

        Config ??= new();
    }

    public void Save()
    {
        try
        {
            using var writer = new StreamWriter(_configFile);
            ConfigYamlStaticContext.Serializer.Serialize(writer, Config);
        }
        catch (Exception e)
        {
            Log.Error("Failed to save config file. Error: {ErrorMessage}", e.Message);
        }
    }
}
