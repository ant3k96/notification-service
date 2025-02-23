namespace Notification.Api.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string? message) : base(message) { }
    }
}
