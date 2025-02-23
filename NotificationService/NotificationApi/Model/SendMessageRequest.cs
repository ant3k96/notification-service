using MediatR;
using System.Text.Json.Serialization;

namespace Notification.Api.Model
{
    public class SendMessageRequest : INotification
    {
        [JsonPropertyName("sms")]
        public PhoneSpecification Sms { get; init; } = default!;

        [JsonPropertyName("email")]
        public EmailSpecification Email { get; init; } = default!;

        [JsonPropertyName("body")]
        public required string Body { get; init; }

    }

    public class EmailSpecification
    {
        [JsonPropertyName("emailTo")]
        public string To { get; init; } = default!;

        [JsonPropertyName("emailFrom")]
        public string From { get; init; } = default!;

        [JsonPropertyName("subject")]
        public string Subject { get; init; } = default!;

    }

    public class PhoneSpecification
    {
        [JsonPropertyName("to")]
        public string To { get; init; } = default!;

        [JsonPropertyName("from")]
        public string From { get; init; } = default!;

    }

}