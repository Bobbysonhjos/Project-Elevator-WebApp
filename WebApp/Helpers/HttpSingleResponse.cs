namespace WebApp.Helpers
{
    public class HttpSingleResponse<T> where T : class
    {
        public PaginationMetadata PaginationMetadata { get; set; } = null!;
        public T Data { get; set; } = null!;
    }
}
