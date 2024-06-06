using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Services
{
    public class ElasticService
    {
        private readonly IElasticClient _elastic;
        private readonly string _indexname = "blog_posts2";

        public ElasticService(IElasticClient elastic)
        {
            _elastic = elastic;
        }


        public async Task<bool> Approve(string id)
        {
            var blogRequest = await _elastic.GetAsync<BlogPost>(id);

            if (!blogRequest.IsValid)
                return false;

            var blog = blogRequest.Source;

            if (blog.IsApproved)
                return false;

            blog.IsApproved = true;

            var request = await _elastic.IndexDocumentAsync(blog);

            if (!request.IsValid)
                return false;

            return true;
        } 


        public async Task<List<BlogPost>> GetAsync()
        {
            var request = new
            {
                query = new
                {
                    match_all = new { }
                }
            };

            var response = await _elastic.SearchAsync<BlogPost>(s =>
            
                s.Index("blog_posts2")
                .Query(q => q.MatchAll())
            );
            
            
            
            if (response.IsValid)
            {
                var rets = response.Hits.Select(hit =>
                {
                    var blog = hit.Source;
                    blog.Id = hit.Id;
                    return blog;
                }).ToList();
                return rets;
            }
            return null;
        }


        public async Task<string> InsertAsync(BlogPost blogPost)
        {
            var input = blogPost;
            input.Id = null;
            var response = await _elastic.IndexDocumentAsync(blogPost);


            input.Id = response.Id;

            return response.Id;
        }


        public async Task DeleteAsync(string id)
        {
            var response = await _elastic.DeleteAsync<BlogPost>(id);
        }

       public async Task<List<BlogPost>> SearchContent(string content)
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
            .Query(q => q
            .Match(m => m
            .Field(f => f.Blog_Content)
            .Query(content))));

            if (response.IsValid)
            {
                var rets = response.Hits.Select(hit =>
                {
                    var blog = hit.Source;
                    blog.Id = hit.Id;
                    return blog;
                }).ToList();
                return rets;
            }
            return null;
        }

        public async Task<List<BlogPost>> SearchCategory(string content)
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
            .Query(q => q
            .Match(m => m
            .Field(f => f.Categories)
            .Query(content))));

            if (response.IsValid)
            {
                var rets = response.Hits.Select(hit =>
                {
                    var blog = hit.Source;
                    blog.Id = hit.Id;
                    return blog;
                }).ToList();
                return rets;
            }

            return null;
        }

        public async Task<BlogPost> SearchBlog(string id)
        {
            var response = await _elastic.GetAsync<BlogPost>(id);

            if (response.IsValid) return response.Source;

            return null;
        }


        public async Task UpdateBlog(BlogPost bp)
        {
            await _elastic.IndexDocumentAsync(bp);

        }
    }
}
