using Elasticsearch.Net;
using Humanizer;
using Microsoft.Extensions.Caching.Distributed;
using Nest;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Models.DTO_s;

namespace WebApplication1.Controllers.Services
{
    public class ElasticService
    {
        private readonly IElasticClient _elastic;
        private readonly IDistributedCache _redisCache;
        private readonly string _indexname = "blog_posts4";

        public ElasticService(IElasticClient elastic, IDistributedCache distributedCache)
        {
            _elastic = elastic;
            _redisCache = distributedCache;
        }


        public async Task<bool> Approve(AprooveDTO dto)
        {
            var token = _redisCache.GetString(dto.UserId.ToString());

            if(token == null) { return false; }

            var jwt = new JwtSecurityToken(token);

            foreach(var claim in jwt.Claims)
            {
                if(claim.Type == ClaimTypes.Role)
                {
                    if (claim.Value == "False") return false;
                }
            }

            var blogRequest = await _elastic.GetAsync<BlogPost>(dto.BlogId);

            if (!blogRequest.IsValid)
                return false;

            var blog = blogRequest.Source;

            if (blog.IsApproved)
                return false;

            blog.IsApproved = true;

            var request = await _elastic.UpdateAsync<BlogPost>(blogRequest.Id, bp => bp
            .Doc(blog)
            );


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
            
                s.Index(_indexname)
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

            var token = _redisCache.GetString(blogPost.UserId.ToString());

            if (token == null) { return "BAD USER"; }

            var jwt = new JwtSecurityToken(token);

            foreach (var claim in jwt.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    if (claim.Value == "False") 
                        blogPost.IsApproved = false;
                    else 
                        blogPost.IsApproved = true;

                }
            }

            blogPost.Timestamp = DateTime.Now;

            var response = await _elastic.IndexDocumentAsync(blogPost);


            input.Id = response.Id;

            return response.Id;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var response = await _elastic.DeleteAsync<BlogPost>(id);
            if (response.IsValid) return true;
            return false;
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

        public async Task<List<BlogPost>> SearchTitle(string title)
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
            .Query(q => q
            .Match(m => m
            .Field(f => f.Short_Description)
            .Query(title))));

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

        public async Task<List<BlogPost>> SearchBlogs(SearchQueryDTO query)
        {
            var response = await _elastic.SearchAsync<BlogPost>(blog => blog
            .Query(q => q
            .Bool(b => b
            .Must(
                mu => mu.Match(m => m.Field(b => b.Blog_Content).Query(query.Content)),
                mu => mu.Match(m => m.Field(b => b.Short_Description).Query(query.Title))
                ))));

            if (response.IsValid)
            {
                var rets = response.Hits.Select(hit =>
                {
                    var blog = hit.Source;
                    blog.Id = hit.Id;
                    
                    return blog;
                    
                }).ToList();

                

                return rets.Where(blog => blog.IsApproved).ToList();
            }
            return null;
        }

        public async Task<List<BlogPost>> FindApproved()
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
            .Query(q => q
            .Bool(m => m
            .Filter(s => s
            .Term(t => t
            .Field(f => f.IsApproved)
            .Value(true))))));

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

        public async Task<List<BlogPost>> FindNotApproved()
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
            .Query(q => q
            .Bool(m => m
            .Filter(s => s
            .Term(t => t
            .Field(f => f.IsApproved)
            .Value(false))))));

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


        public async Task<List<BlogPost>> GetMyBlogs(string id)
        {
            var response = await _elastic.SearchAsync<BlogPost>(s => s
           .Query(q => q
           .Match(m => m
           .Field(f => f.UserId)
           .Query(id))));

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

        public async Task<bool> UpdateBlog(BlogPost bp)
        {
            var blogRequest = await _elastic.GetAsync<BlogPost>(bp.Id);

            if (!blogRequest.IsValid)
                return false;

            var blog = blogRequest.Source;



            blog.Short_Description = bp.Short_Description;
            blog.Blog_Content = bp.Blog_Content;
            blog.Categories = bp.Categories;


            var request = await _elastic.UpdateAsync<BlogPost>(blogRequest.Id, bp => bp
            .Doc(blog)
            );

            return request.IsValid;

        }
    }
}
