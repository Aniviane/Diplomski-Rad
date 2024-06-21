namespace WebApplication1.Models.DTO_s
{
    public class AprooveDTO
    {
        public AprooveDTO()
        {
        }

        public AprooveDTO(Guid userId, string blogId)
        {
            UserId = userId;
            BlogId = blogId;
        }

        public Guid UserId { get; set; }

        public string BlogId { get; set; }


    }
}
