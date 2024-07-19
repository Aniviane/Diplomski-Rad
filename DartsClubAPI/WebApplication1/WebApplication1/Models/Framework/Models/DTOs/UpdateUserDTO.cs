namespace WebApplication1.Models.Framework.Models.DTOs
{
    public class UpdateUserDTO
    {
        public UpdateUserDTO(Guid? id, string oldPassword, string newPassword)
        {
            Id = id;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public Guid? Id { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
