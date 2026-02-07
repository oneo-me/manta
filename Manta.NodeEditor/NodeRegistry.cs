using System.Reflection;
using Manta.NodeEditor.Attributes;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor;

public sealed class NodeRegistry
{
    sealed class NodeTypeRecord
    {
        public required Type NodeType { get; init; }
        public required string DisplayName { get; init; }
        public required string Category { get; init; }
        public required List<PortRecord> InputPorts { get; init; }
        public PortRecord? OutputPort { get; init; }
    }

    sealed class PortRecord
    {
        public required string PropertyName { get; init; }
        public required string DisplayName { get; init; }
        public required Type DataType { get; init; }
        public required int Order { get; init; }
    }

    readonly Dictionary<Type, NodeTypeRecord> _types = new();
    readonly Dictionary<string, List<NodeTypeRecord>> _categories = new();

    public void ScanAssembly(Assembly assembly)
    {
        var nodeTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.IsSubclassOf(typeof(NodeBase))
                        && t.GetCustomAttribute<NodeAttribute>() is not null);

        foreach (var type in nodeTypes)
            RegisterType(type);
    }

    void RegisterType(Type type)
    {
        if (_types.ContainsKey(type)) return;

        var nodeAttr = type.GetCustomAttribute<NodeAttribute>()!;
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var inputs = new List<PortRecord>();
        PortRecord? output = null;

        foreach (var prop in properties)
        {
            var inputAttr = prop.GetCustomAttribute<NodeInputAttribute>();
            if (inputAttr is not null)
            {
                inputs.Add(new PortRecord
                {
                    PropertyName = prop.Name,
                    DisplayName = inputAttr.DisplayName ?? prop.Name,
                    DataType = prop.PropertyType,
                    Order = inputAttr.Order
                });
            }

            var outputAttr = prop.GetCustomAttribute<NodeOutputAttribute>();
            if (outputAttr is not null)
            {
                output = new PortRecord
                {
                    PropertyName = prop.Name,
                    DisplayName = outputAttr.DisplayName ?? prop.Name,
                    DataType = prop.PropertyType,
                    Order = 0
                };
            }
        }

        inputs = inputs.OrderBy(p => p.Order).ToList();

        var record = new NodeTypeRecord
        {
            NodeType = type,
            DisplayName = nodeAttr.DisplayName ?? type.Name,
            Category = nodeAttr.Category,
            InputPorts = inputs,
            OutputPort = output
        };

        _types[type] = record;

        if (!_categories.TryGetValue(record.Category, out var list))
        {
            list = [];
            _categories[record.Category] = list;
        }

        list.Add(record);
    }

    public NodeBase CreateNode(Type nodeType, double x = 0, double y = 0)
    {
        if (!_types.TryGetValue(nodeType, out var record))
            throw new ArgumentException($"Node type {nodeType.Name} is not registered");

        var node = (NodeBase)Activator.CreateInstance(nodeType)!;
        node.X = x;
        node.Y = y;
        node.Category = record.Category;

        foreach (var portRecord in record.InputPorts)
        {
            var port = new NodePort
            {
                Name = portRecord.PropertyName,
                DisplayName = portRecord.DisplayName,
                DataType = portRecord.DataType,
                Direction = PortDirection.Input,
                Order = portRecord.Order,
                Owner = node
            };
            node.Inputs.Add(port);
        }

        if (record.OutputPort is not null)
        {
            node.Output = new NodePort
            {
                Name = record.OutputPort.PropertyName,
                DisplayName = record.OutputPort.DisplayName,
                DataType = record.OutputPort.DataType,
                Direction = PortDirection.Output,
                Order = 0,
                Owner = node
            };
        }

        return node;
    }

    public T CreateNode<T>(double x = 0, double y = 0) where T : NodeBase
        => (T)CreateNode(typeof(T), x, y);

    public IReadOnlyDictionary<string, List<Type>> GetCategorizedTypes()
    {
        return _categories.ToDictionary(
            kv => kv.Key,
            kv => kv.Value.Select(r => r.NodeType).ToList());
    }

    public string GetDisplayName(Type nodeType)
        => _types.TryGetValue(nodeType, out var record) ? record.DisplayName : nodeType.Name;

    public bool IsRegistered(Type nodeType) => _types.ContainsKey(nodeType);
}
