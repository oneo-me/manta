using System.Diagnostics.CodeAnalysis;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace AirKitDemo.Extensions;

static class YamlExtensions
{
    static readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
        .WithTypeConverter(new DateTimeConverter())
        .WithTypeConverter(new DateTimeOffsetConverter())
        .WithTypeConverter(new DateOnlyConverter())
        .WithTypeConverter(new TimeOnlyConverter())
        .Build();

    static readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .WithTypeConverter(new DateTimeConverter())
        .WithTypeConverter(new DateTimeOffsetConverter())
        .WithTypeConverter(new DateOnlyConverter())
        .WithTypeConverter(new TimeOnlyConverter())
        .Build();

    public static string ConvertYamlName(this string name)
    {
        return UnderscoredNamingConvention.Instance.Apply(name.Replace(".", string.Empty, StringComparison.Ordinal));
    }

    public static string SerializeYaml<T>(this T value)
    {
        return _serializer.Serialize(value);
    }

    [SuppressMessage("ReSharper", "NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract")]
    public static T DeserializeYaml<T>(this string yml) where T : class, new()
    {
        return _deserializer.Deserialize<T>(yml) ?? new();
    }

    public static object? DeserializeYaml(this string yml, Type type)
    {
        return _deserializer.Deserialize(yml, type);
    }
}
