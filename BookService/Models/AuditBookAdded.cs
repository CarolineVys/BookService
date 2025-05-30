namespace BookService.Models
{
    public class AuditBookAdded
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
