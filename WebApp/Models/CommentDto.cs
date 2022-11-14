namespace WebApp.Models
{
    public class CommentDto
    { 
        public string Id { get; init; } = null!;
        public string CreatedById { get; init; } = null!;
        public string CreatedByName { get; init; } = null!;
        public DateTime CreatedDateUtc { get; init; }
        public string Message { get; init; } = null!;
    }
}
