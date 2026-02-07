using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Execution;

public static class TopologicalSorter
{
    public static List<NodeBase> Sort(NodeGraph graph)
    {
        var result = new List<NodeBase>();
        var visited = new HashSet<NodeBase>();
        var temp = new HashSet<NodeBase>();

        foreach (var node in graph.Nodes)
        {
            if (!visited.Contains(node))
                Visit(graph, node, visited, temp, result);
        }

        result.Reverse();
        return result;
    }

    static void Visit(NodeGraph graph, NodeBase node, HashSet<NodeBase> visited,
        HashSet<NodeBase> temp, List<NodeBase> result)
    {
        if (temp.Contains(node))
            throw new InvalidOperationException("Graph contains a cycle");

        if (visited.Contains(node))
            return;

        temp.Add(node);

        var incoming = graph.Connections.Where(c => c.TargetNode == node);
        foreach (var conn in incoming)
        {
            if (conn.SourceNode is not null)
                Visit(graph, conn.SourceNode, visited, temp, result);
        }

        temp.Remove(node);
        visited.Add(node);
        result.Add(node);
    }
}
