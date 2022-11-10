namespace WebApp.Helpers
{
    public class HttpResponse<T> where T : class
    {
        public PaginationMetadata PaginationMetadata { get; set; } = null!;
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
