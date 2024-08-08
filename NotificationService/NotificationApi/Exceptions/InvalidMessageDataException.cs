namespace Notification.Api.Exceptions
{
    public class InvalidMessageDataException : Exception
    {
        public InvalidMessageDataException(string? message) : base(message) { }
    }
}
