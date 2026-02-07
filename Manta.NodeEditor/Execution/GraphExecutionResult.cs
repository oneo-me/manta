using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Execution;

public sealed class GraphExecutionResult
{
    public bool IsSuccess { get; init; }
    public Exception? Exception { get; init; }
    public NodeBase? FailedNode { get; init; }
    public string? ErrorMessage { get; init; }
    public TimeSpan ExecutionTime { get; init; }

    public static GraphExecutionResult Success(TimeSpan time) =>
        new() { IsSuccess = true, ExecutionTime = time };

    public static GraphExecutionResult Failure(NodeBase node, Exception ex) =>
        new()
        {
            IsSuccess = false,
            FailedNode = node,
            Exception = ex,
            ErrorMessage = $"Node '{node.DisplayName}' failed: {ex.Message}"
        };
}
