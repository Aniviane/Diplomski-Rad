namespace WebApplication1.Models.Framework.Models.DTOs
{
    public class UserCreateDTO
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool isModerator { get; set; }
    }
}
