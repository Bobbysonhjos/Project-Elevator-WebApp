namespace WebApp.Models
{
    public class UserDto
    {
        public string Id { get; set; } 
        public string Role { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
      
    }
}
