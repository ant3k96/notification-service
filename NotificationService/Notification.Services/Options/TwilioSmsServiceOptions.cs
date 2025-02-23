namespace Notification.Services.Options
{
    public class TwilioSmsServiceOptions
    {
        public const string SectionName = "TwillioSms";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
    }
}
