using MediatR;
using System.Text.Json.Serialization;

namespace Notification.Api.Model
{
    public class SendMessageRequest : INotification
    {
        [JsonPropertyName("from")]
        public required string From { get; init; }

        [JsonPropertyName("to")]
        public required string To { get; init; }

        [JsonPropertyName("body")]
        public required string Body { get; init; }

    }
}