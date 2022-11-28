namespace WebApp.Models
{
    public class UserDto
    {
        public string Id { get; set; } 
        public string Role { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
