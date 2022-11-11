namespace WebApp.Helpers
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; set; } = 0;
        public int TotalPageCount { get; set; } = 0;
        public int PageSize { get; set; } = 1;
        public int CurrentPage { get; set; } = 1;
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPageCount;
    }
}
