using System.Text.Json;
using Axe.Logging;
using AxeDemo.Models;

namespace AxeDemo.Services;

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
                var configJson = File.ReadAllText(_configFile);
                var config = JsonSerializer.Deserialize<Config>(configJson);
                if (config is not null)
                    Config = config;
            }
        }
        catch (Exception e)
        {
            Log.Error("Load config error: {0}", e.Message);
        }

        Config ??= new();
    }

    public void Save()
    {
        try
        {
            var configJson = JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFile, configJson);
        }
        catch (Exception e)
        {
            Log.Error("Save config error: {0}", e.Message);
        }
    }
}
