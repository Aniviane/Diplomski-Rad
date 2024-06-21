using WebApplication1.Models.Framework.Models.DTOs;

namespace WebApplication1.Models.Framework.Models
{
    public class User
    {
      

        public Guid? ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool isModerator { get; set; }


        public List<Reservation> Reservations { get; set; }

        public Picture Picture { get; set; }

        public User() { }

        public User(UserCreateDTO userCreateDTO)
        {
            ID = null;
            Name = userCreateDTO.Name;
            Email = userCreateDTO.Email;
            isModerator = userCreateDTO.isModerator;
            Password = userCreateDTO.Password;
            Reservations = new List<Reservation>();
        }

        public User(string name, string email, string password, bool ismoderator)
        {
            ID = null;
            Name = name;
            Email = email;
            Password = password;
            isModerator = ismoderator;
            Reservations = new List<Reservation>();
            
            
        }
    }
}
