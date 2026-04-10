using System.Text.Json;
using Manta.Logging;
using MantaDemo.Models;

namespace MantaDemo.Services;

sealed class ConfigService : IDisposable
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
                var json = File.ReadAllText(_configFile);
                Config = JsonSerializer.Deserialize(json, ConfigJsonSerializerContext.Default.Config) ?? throw new InvalidOperationException("Deserialized config is null");
            }
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load config file, using default config. Error: {ErrorMessage}", e.Message);
        }

        Config ??= new();
    }

    public void Dispose()
    {
        try
        {
            var json = JsonSerializer.Serialize(Config, ConfigJsonSerializerContext.Default.Config);
            File.WriteAllText(_configFile, json);
        }
        catch (Exception e)
        {
            Log.Error("Failed to save config file. Error: {ErrorMessage}", e.Message);
        }
    }
}
