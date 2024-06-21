namespace WebApplication1.Models.Framework.Models.DTOs
{
    public class UserDTO
    {

        public Guid? Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool isModerator { get; set; }

        public List<ReservationDTO> Reservations { get; set; }

        public Picture Picture { get; set; }

        public UserDTO(User user)
        {
            this.Id = user.ID;
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = user.Password;
            this.isModerator = user.isModerator;
            this.Reservations = new List<ReservationDTO>();
            foreach(var reservation in user.Reservations)
            {
                this.Reservations.Add(new ReservationDTO(reservation));
            }
            this.Picture = user.Picture;
        }
    }
}
