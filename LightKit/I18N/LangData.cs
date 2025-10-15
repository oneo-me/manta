using Avalonia.Platform;
using LightKit.I18N.Core;

namespace LightKit.I18N;

public sealed class LangData : LangDataBase
{
    readonly Func<object, string?> _getKey;

    LangData(LangType? defCulture, Func<object, string?> getKey) : base(defCulture)
    {
        _getKey = getKey;
    }

    readonly Dictionary<LangType, Dictionary<string, string>> _items = new();

    protected override string? GetOverride(LangType langType, object key)
    {
        var strKey = _getKey(key);
        if (strKey is null)
            return null;

        if (_items.TryGetValue(langType, out var item))
            if (item.TryGetValue(strKey, out var value))
                return value;

        return null;
    }

    public static LangDataBase LoadFromAssembly<TKey>() where TKey : Enum
    {
        var assembly = typeof(TKey).Assembly;
        var configUri = new Uri($"avares://{assembly.GetName().Name}/Langs/@config.yml");
        var config = new LangConfig();
        if (AssetLoader.Exists(configUri))
        {
            using var stream = AssetLoader.Open(configUri);
            using var reader = new StreamReader(stream);
            var yaml = reader.ReadToEnd();
            config = LangConfig.Load(yaml);
        }

        var result = new LangData(config.Base, x => x is TKey k ? k.ToString() : null);

        foreach (var culture in Enum.GetValues<LangType>())
        {
            var uri = new Uri($"avares://{assembly.GetName().Name}/Langs/{culture}.yml");

            if (AssetLoader.Exists(uri) is false)
                continue;

            using var stream = AssetLoader.Open(uri);
            using var reader = new StreamReader(stream);
            var yaml = reader.ReadToEnd();
            result._items[culture] = yaml.DeserializeYaml<Dictionary<string, string>>();
        }

        return result;
    }
}
