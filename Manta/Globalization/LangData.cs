using System.Reflection;
using Avalonia.Platform;
using Manta.Globalization.Core;

namespace Manta.Globalization;

public sealed class LangData : LangDataBase
{
    LangData(LangType defCulture) : base(defCulture)
    {
    }

    readonly Dictionary<LangType, Dictionary<string, string>> _items = new();

    protected override string? GetOverride(LangType langType, string key)
    {
        if (_items.TryGetValue(langType, out var item))
            if (item.TryGetValue(key, out var value))
                return value;

        return null;
    }

    public static LangDataBase LoadFromAssembly(Assembly assembly)
    {
        var assemblyName = assembly.GetName().Name;
        if (assemblyName is null)
            throw new InvalidOperationException("Assembly must have a valid name.");

        var configUri = new Uri($"avares://{assemblyName}/Langs/@config.yml");
        var config = new LangConfig();
        if (AssetLoader.Exists(configUri))
        {
            using var stream = AssetLoader.Open(configUri);
            using var reader = new StreamReader(stream);
            var yaml = reader.ReadToEnd();
            config = LangConfig.Load(yaml);
        }

        var result = new LangData(config.Base);

        foreach (var culture in Enum.GetValues<LangType>())
        {
            var uri = new Uri($"avares://{assemblyName}/Langs/{culture}.yml");

            if (AssetLoader.Exists(uri) is false)
                continue;

            using var stream = AssetLoader.Open(uri);
            using var reader = new StreamReader(stream);
            var yaml = reader.ReadToEnd();
            var deserializeYaml = yaml.LoadData();
            result._items[culture] = new();
            foreach (var keyValuePair in deserializeYaml)
                result._items[culture][Lang.CreateKey(assemblyName, keyValuePair.Key)] = keyValuePair.Value;
        }

        return result;
    }
}
