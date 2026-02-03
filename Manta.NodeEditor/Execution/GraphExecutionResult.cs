using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Execution;

public sealed class GraphExecutionResult
{
    public bool IsSuccess { get; init; }
    public Exception? Exception { get; init; }
    public Node? FailedNode { get; init; }
    public string? ErrorMessage { get; init; }
    public TimeSpan ExecutionTime { get; init; }

    public static GraphExecutionResult Success(TimeSpan executionTime)
    {
        return new()
        {
            IsSuccess = true,
            ExecutionTime = executionTime
        };
    }

    public static GraphExecutionResult Failure(Node node, Exception exception)
    {
        return new()
        {
            IsSuccess = false,
            FailedNode = node,
            Exception = exception,
            ErrorMessage = $"Node '{node.DisplayName}' failed: {exception.Message}"
        };
    }
}
