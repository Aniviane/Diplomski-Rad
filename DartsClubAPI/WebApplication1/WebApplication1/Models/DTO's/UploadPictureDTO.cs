namespace WebApplication1.Models.DTO_s
{
    public class UploadPictureDTO
    {
        public Guid UserId {  get; set; }

        public IFormFile Picture { get; set; }
    }
}
