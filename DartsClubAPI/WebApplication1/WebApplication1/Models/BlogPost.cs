namespace WebApplication1.Models
{
    public class BlogPost
    {
        public BlogPost(string news_Site, string short_Description, int word_Count)
        {
            News_Site = news_Site;
            Short_Description = short_Description;
            Word_Count = word_Count;
        }

        public string News_Site { get; set; }
        public string Short_Description { get; set; }

        public int Word_Count { get; set; }
    }
}
