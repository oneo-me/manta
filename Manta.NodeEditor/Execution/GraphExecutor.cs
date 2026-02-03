using System.Diagnostics;
using Manta.NodeEditor.Models;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Execution;

public sealed class GraphExecutor(TypeCompatibility typeCompatibility)
{
    readonly GraphValidator _validator = new(typeCompatibility);

    public event Action<Node>? NodeExecuting;
    public event Action<Node>? NodeExecuted;

    public GraphValidationResult Validate(Graph graph)
    {
        return _validator.Validate(graph);
    }

    public async Task<GraphExecutionResult> ExecuteAsync(Graph graph, CancellationToken cancellationToken = default)
    {
        // Validate first
        var validationResult = Validate(graph);
        if (!validationResult.IsValid)
            return new()
            {
                IsSuccess = false,
                ErrorMessage = string.Join("; ", validationResult.Errors)
            };

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Get execution order
            var executionOrder = TopologicalSorter.Sort(graph);

            // Execute nodes in order
            foreach (var node in executionOrder)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    NodeExecuting?.Invoke(node);

                    // Transfer input values from connected output ports
                    TransferInputValues(graph, node);

                    // Execute the node
                    await node.Instance.ExecuteAsync(cancellationToken);

                    // Read output values from node properties to ports
                    TransferOutputValues(node);

                    NodeExecuted?.Invoke(node);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    return GraphExecutionResult.Failure(node, ex);
                }
            }

            stopwatch.Stop();
            return GraphExecutionResult.Success(stopwatch.Elapsed);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                Exception = ex
            };
        }
    }

    void TransferInputValues(Graph graph, Node node)
    {
        foreach (var inputPort in node.InputPorts)
        {
            var connection = graph.Connections.FirstOrDefault(c => c.TargetPort == inputPort);
            if (connection != null)
            {
                var sourceValue = connection.SourcePort.Value;
                var sourceType = connection.SourcePort.DataType;
                var targetType = inputPort.DataType;

                var convertedValue = typeCompatibility.ConvertValue(sourceValue, sourceType, targetType);
                inputPort.Value = convertedValue;

                // Also set the property on the node instance
                var prop = node.Instance.GetType().GetProperty(inputPort.Name);
                prop?.SetValue(node.Instance, convertedValue);
            }
        }
    }

    void TransferOutputValues(Node node)
    {
        foreach (var outputPort in node.OutputPorts)
        {
            var prop = node.Instance.GetType().GetProperty(outputPort.Name);
            if (prop != null)
                outputPort.Value = prop.GetValue(node.Instance);
        }
    }
}
