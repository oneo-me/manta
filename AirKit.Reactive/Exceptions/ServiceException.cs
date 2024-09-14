namespace AirKit.Reactive.Exceptions;

[Serializable]
public sealed class ServiceException : Exception
{
    public ServiceException()
    {
    }

    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
