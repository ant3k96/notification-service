using MediatR;
using System.Text.Json.Serialization;

namespace Notification.Api.Model
{
    public class SendMessageRequest : INotification
    {
        [JsonPropertyName("from")]
        public required string From { get; init; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; init; } = default!;

        [JsonPropertyName("email")]
        public string Email { get; init; } = default!;

        [JsonPropertyName("body")]
        public required string Body { get; init; }

    }
}