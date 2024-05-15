using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Services
{
    public class ElasticService
    {
        private readonly IElasticClient _elastic;
        private readonly string _indexname = "blog_articles";

        public ElasticService(IElasticClient elastic)
        {
            _elastic = elastic;
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
            
                s.Index("blog_articles")
                .Query(q => q.MatchAll())
            );
            
            
            
            if (response.IsValid)
            {
                return response.Documents.ToList();
            }
            return null;
        }

    }
}
