namespace EmailWorker.Models
{
    public class EmailMessage
    {
        public string Subject { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string ToName { get; set; } = null!;

        public string ToEmail { get; set; } = null!;
    }
}
