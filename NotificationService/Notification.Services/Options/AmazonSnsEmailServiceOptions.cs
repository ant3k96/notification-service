namespace Notification.Services.Options
{
    public class AmazonSnsEmailServiceOptions
    {
        public const string SectionName = "AmazonSnsEmail";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
    }
}
