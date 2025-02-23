namespace Notification.Services.Options
{
    public class TwilioSendGridEmailOptions
    {
        public const string SectionName = "TwilioSendGridEmail";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
    }
}
