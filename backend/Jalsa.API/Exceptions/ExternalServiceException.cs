namespace Jalsa.API.Exceptions;

public class ExternalServiceException : Exception
{
    public int UpstreamStatusCode { get; }

    public ExternalServiceException(int upstreamStatusCode, string message)
        : base(message)
    {
        UpstreamStatusCode = upstreamStatusCode;
    }

    public ExternalServiceException(int upstreamStatusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        UpstreamStatusCode = upstreamStatusCode;
    }
}
