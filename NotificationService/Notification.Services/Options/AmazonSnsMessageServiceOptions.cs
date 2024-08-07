namespace Notification.Services.Options
{
    public class AmazonSnsMessageServiceOptions
    {
        public const string SectionName = "AmazonSns";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
        //public string AccountSid { get; set; } = default!;
        //public string AuthToken { get; set; } = default!;
    }
}
