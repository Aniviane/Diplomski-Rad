using Nest;

namespace WebApplication1.Models
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class BlogPost
    {
        

        public BlogPost(string id, DateTime timestamp, string categories, string short_Description, string userId, bool isApproved, string blog_Content, int word_Count)
        {
            Id = id;
            Short_Description = short_Description;
            UserId = userId;
            Timestamp = timestamp;
            IsApproved = isApproved;
            Blog_Content = blog_Content;
            Word_Count = word_Count;
            Categories = categories;
        }

        [PropertyName("_id")]
        public string Id { get; set; }
        public string Short_Description { get; set; }

        public DateTime Timestamp { get; set; }
        public string Categories { get; set; }
        public string UserId { get; set; }

        public bool IsApproved { get; set; }
        public string Blog_Content { get; set; }
        public int Word_Count { get; set; }
    }
}
