namespace ApplicationCore.DTO
{
    public class Page<T>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int MaxCount { get; set; }
        public List<T> Elements { get; set; } = null!;

        public int? Offset => (PageNumber - 1) * PageSize;
    }
}