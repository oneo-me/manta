using System.Diagnostics;
using System.Reflection;
using Manta.NodeEditor.Models;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Execution;

public sealed class GraphExecutor(TypeCompatibility typeCompatibility)
{
    public async Task<GraphExecutionResult> ExecuteAsync(
        NodeGraph graph, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var order = TopologicalSorter.Sort(graph);

            foreach (var node in order)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    TransferInputs(graph, node);
                    await node.ExecuteAsync(cancellationToken);
                    TransferOutputs(node);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    return GraphExecutionResult.Failure(node, ex);
                }
            }

            sw.Stop();
            return GraphExecutionResult.Success(sw.Elapsed);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new GraphExecutionResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                Exception = ex
            };
        }
    }

    void TransferInputs(NodeGraph graph, NodeBase node)
    {
        foreach (var inputPort in node.Inputs)
        {
            var conn = graph.Connections.FirstOrDefault(c => c.Target == inputPort);
            if (conn is null) continue;

            var value = conn.Source.Value;
            var converted = typeCompatibility.ConvertValue(value, conn.Source.DataType, inputPort.DataType);
            inputPort.Value = converted;

            var prop = node.GetType().GetProperty(inputPort.Name,
                BindingFlags.Public | BindingFlags.Instance);
            if (prop?.CanWrite == true)
                prop.SetValue(node, converted);
        }
    }

    static void TransferOutputs(NodeBase node)
    {
        if (node.Output is null) return;

        var prop = node.GetType().GetProperty(node.Output.Name,
            BindingFlags.Public | BindingFlags.Instance);

        if (prop is not null)
            node.Output.Value = prop.GetValue(node);
    }
}
