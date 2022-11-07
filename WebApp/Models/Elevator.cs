namespace WebApp.Models
{
    public class Elevator
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Ongoing { get; set; } = null!;
        public string Completed { get; set; } = null!;

        public List<Elevator> Elevators { get; set; } = new List<Elevator>();

    }
}
