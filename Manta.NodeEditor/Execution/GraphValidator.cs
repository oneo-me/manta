using Manta.NodeEditor.Models;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Execution;

public sealed class GraphValidator(TypeCompatibility typeCompatibility)
{
    public GraphValidationResult Validate(Graph graph)
    {
        var errors = new List<string>();

        // Check for cycles
        if (HasCycle(graph))
            errors.Add("Cycle detected in graph");

        // Check type compatibility for all connections
        foreach (var connection in graph.Connections)
        {
            var sourceType = connection.SourcePort.DataType;
            var targetType = connection.TargetPort.DataType;

            if (!typeCompatibility.AreTypesCompatible(sourceType, targetType))
                errors.Add($"Type mismatch: Node '{connection.SourceNode.DisplayName}' port '{connection.SourcePort.DisplayName}' ({sourceType.Name}) " +
                           $"is not compatible with Node '{connection.TargetNode.DisplayName}' port '{connection.TargetPort.DisplayName}' ({targetType.Name})");
        }

        return errors.Count > 0
            ? GraphValidationResult.Failure([..errors])
            : GraphValidationResult.Success();
    }

    bool HasCycle(Graph graph)
    {
        var visited = new HashSet<Node>();
        var recursionStack = new HashSet<Node>();

        foreach (var node in graph.Nodes)
            if (HasCycleDfs(graph, node, visited, recursionStack))
                return true;

        return false;
    }

    bool HasCycleDfs(Graph graph, Node node, HashSet<Node> visited, HashSet<Node> recursionStack)
    {
        if (recursionStack.Contains(node))
            return true;

        if (visited.Contains(node))
            return false;

        visited.Add(node);
        recursionStack.Add(node);

        // Find all nodes that this node outputs to
        var outgoingConnections = graph.Connections.Where(c => c.SourceNode == node);
        foreach (var connection in outgoingConnections)
            if (HasCycleDfs(graph, connection.TargetNode, visited, recursionStack))
                return true;

        recursionStack.Remove(node);
        return false;
    }
}
