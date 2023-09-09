namespace Chat.Helper
{
    public class getchathistory
    {
        public long? TogroupId { get; set; }
        public long? ToUserId { get; set; }
        public int? skip { get; set; } = null;
        public int? take { get; set; } = null;
    }
}
