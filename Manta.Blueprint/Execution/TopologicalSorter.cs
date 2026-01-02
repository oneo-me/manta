using Manta.Blueprint.Models;

namespace Manta.Blueprint.Execution;

public static class TopologicalSorter
{
    public static List<Node> Sort(Graph graph)
    {
        var result = new List<Node>();
        var visited = new HashSet<Node>();
        var tempVisited = new HashSet<Node>();

        foreach (var node in graph.Nodes)
            if (!visited.Contains(node))
                Visit(graph, node, visited, tempVisited, result);

        result.Reverse();
        return result;
    }

    static void Visit(Graph graph, Node node, HashSet<Node> visited, HashSet<Node> tempVisited, List<Node> result)
    {
        if (tempVisited.Contains(node))
            throw new InvalidOperationException("Graph contains a cycle");

        if (visited.Contains(node))
            return;

        tempVisited.Add(node);

        // Visit all nodes that provide input to this node
        var incomingConnections = graph.Connections.Where(c => c.TargetNode == node);
        foreach (var connection in incomingConnections)
            Visit(graph, connection.SourceNode, visited, tempVisited, result);

        tempVisited.Remove(node);
        visited.Add(node);
        result.Add(node);
    }
}
