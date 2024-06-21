namespace WebApplication1.Models.DTO_s
{
    public class LoginDTO
    {
        public LoginDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }

        public string Password { get; set; }


    }
}
