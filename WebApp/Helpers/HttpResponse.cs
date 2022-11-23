namespace WebApp.Helpers
{
    public class HttpResponse<T> where T : class
    {
        public T Data { get; set; } = null!;
    }
}
