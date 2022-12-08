namespace WebApp.Models
{
    public class ElevatorDto
    {
        public string Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}