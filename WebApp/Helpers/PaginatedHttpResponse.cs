namespace WebApp.Helpers
{
    public class PaginatedHttpResponse<T>
    {
        public PaginationMetadata PaginationMetadata { get; set; } = null!;
        public T Data { get; set; } = default!;
    }
}
