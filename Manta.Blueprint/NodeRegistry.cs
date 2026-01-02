using System.Reflection;
using Manta.Blueprint.Attributes;
using Manta.Blueprint.Models;

namespace Manta.Blueprint;

public sealed class NodeRegistry
{
    readonly Dictionary<Type, NodeTypeInfo> _registeredTypes = new();
    readonly Dictionary<string, List<NodeTypeInfo>> _categorizedNodes = new();

    public IReadOnlyDictionary<Type, NodeTypeInfo> RegisteredTypes => _registeredTypes;
    public IReadOnlyDictionary<string, List<NodeTypeInfo>> CategorizedNodes => _categorizedNodes;

    public void ScanAssembly(Assembly assembly)
    {
        var nodeTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(NodeBase)))
            .Where(t => t.GetCustomAttribute<NodeAttribute>() != null);

        foreach (var type in nodeTypes)
            RegisterNodeType(type);
    }

    public void RegisterNodeType(Type type)
    {
        if (_registeredTypes.ContainsKey(type))
            return;

        var nodeAttr = type.GetCustomAttribute<NodeAttribute>();
        if (nodeAttr == null)
            throw new ArgumentException($"Type {type.Name} must have [Node] attribute");

        var inputPorts = new List<PortDefinition>();
        var outputPorts = new List<PortDefinition>();

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var inputAttr = prop.GetCustomAttribute<InputAttribute>();
            if (inputAttr != null)
                inputPorts.Add(new()
                {
                    Name = prop.Name,
                    DisplayName = inputAttr.DisplayName ?? prop.Name,
                    DataType = prop.PropertyType,
                    Direction = PortDirection.Input,
                    Index = inputAttr.Order
                });

            var outputAttr = prop.GetCustomAttribute<OutputAttribute>();
            if (outputAttr != null)
                outputPorts.Add(new()
                {
                    Name = prop.Name,
                    DisplayName = outputAttr.DisplayName ?? prop.Name,
                    DataType = prop.PropertyType,
                    Direction = PortDirection.Output,
                    Index = outputAttr.Order
                });
        }

        inputPorts = inputPorts.OrderBy(p => p.Index).ToList();
        outputPorts = outputPorts.OrderBy(p => p.Index).ToList();

        var typeInfo = new NodeTypeInfo
        {
            NodeType = type,
            DisplayName = nodeAttr.DisplayName ?? type.Name,
            Category = nodeAttr.Category,
            InputPortDefinitions = inputPorts,
            OutputPortDefinitions = outputPorts
        };

        _registeredTypes[type] = typeInfo;

        if (!_categorizedNodes.ContainsKey(typeInfo.Category))
            _categorizedNodes[typeInfo.Category] = [];

        _categorizedNodes[typeInfo.Category].Add(typeInfo);
    }

    public NodeTypeInfo? GetNodeTypeInfo(Type type)
    {
        return _registeredTypes.GetValueOrDefault(type);
    }

    public NodeTypeInfo? GetNodeTypeInfo(string typeFullName)
    {
        return _registeredTypes.Values.FirstOrDefault(t => t.TypeFullName == typeFullName);
    }
}
