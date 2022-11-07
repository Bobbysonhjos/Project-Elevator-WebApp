namespace WebApp.Models
{
    public class ElevatorModel
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Ongoing { get; set; } = null!;
        public string Completed { get; set; } = null!;

    }
}
