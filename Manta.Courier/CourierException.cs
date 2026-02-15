namespace Manta.Courier;

public sealed class CourierException : Exception
{
    public CourierException(string message) : base(message)
    {
    }

    public CourierException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
