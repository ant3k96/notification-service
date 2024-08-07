namespace Notification.Services.Options
{
    public class TwilioMessageServiceOptions
    {
        public const string SectionName = "Twillio";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
        public string AccountSid { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
    }
}
