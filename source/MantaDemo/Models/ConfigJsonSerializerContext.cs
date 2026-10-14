using System.Text.Json.Serialization;
using Manta.Globalization.Core;
using MantaDemo.Reactive;

namespace MantaDemo.Models;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Config))]
[JsonSerializable(typeof(Dictionary<DemoGroup, string>))]
[JsonSerializable(typeof(LangType))]
[JsonSerializable(typeof(DemoGroup))]
sealed partial class ConfigJsonSerializerContext : JsonSerializerContext;
