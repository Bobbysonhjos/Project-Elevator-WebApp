namespace WebApp.Models
{
    public class ErrandDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public string AssignedToId { get; set; } = null!;
        public string AssignedToName { get; set; } = null!;
        public string CreatedById { get; set; } = null!;
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
