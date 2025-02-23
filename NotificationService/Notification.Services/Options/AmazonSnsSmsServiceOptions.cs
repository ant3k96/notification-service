namespace Notification.Services.Options
{
    public class AmazonSnsSmsServiceOptions
    {
        public const string SectionName = "AmazonSnsSms";

        public bool Enabled { get; set; }
        public int Priority { get; set; }
    }
}
