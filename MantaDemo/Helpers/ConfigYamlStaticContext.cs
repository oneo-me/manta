using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MantaDemo.Helpers;

[YamlStaticContext]
sealed partial class ConfigYamlStaticContext
{
    public static IDeserializer Deserializer { get; } = new StaticDeserializerBuilder(new ConfigYamlStaticContext())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public static ISerializer Serializer { get; } = new StaticSerializerBuilder(new ConfigYamlStaticContext())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
}
