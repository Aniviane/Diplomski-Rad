namespace WebApplication1.Models.Framework.Models.DTOs
{
    public class UsernameDTO
    {
        public UsernameDTO()
        {
        }

        public UsernameDTO(Guid? id, string name)
        {
            Id = id;
            Name = name;
        }

        public UsernameDTO(User user)
        {
            Id = user.ID;
            Name = user.Name;
        }

        public Guid? Id { get; set; }

        public string Name {  get; set; }
    }
}
