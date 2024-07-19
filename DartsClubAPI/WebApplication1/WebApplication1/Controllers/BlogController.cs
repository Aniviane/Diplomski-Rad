using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Services;
using WebApplication1.Models;
using WebApplication1.Models.DTO_s;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        private readonly ElasticService _elasticService;

        public BlogController(ElasticService elasticService)
        {
            _elasticService = elasticService;
        }



        // GET: api/<BlogController>
        [HttpGet]
        public async Task<IEnumerable<BlogPost>> Get()
        {
            return await _elasticService.GetAsync();
        }

        [HttpGet("Approved")]
        public async Task<IEnumerable<BlogPost>> GetApproved()
        {
            return await _elasticService.FindApproved();
        }

        [HttpGet("MyBlogs/{id}")]
        public async Task<IEnumerable<BlogPost>> GetMyBlogs(string id)
        {
            return await _elasticService.GetMyBlogs(id);
        }

        [HttpGet("NotApproved")]
        public async Task<IEnumerable<BlogPost>> GetNotApproved()
        {
            return await _elasticService.FindNotApproved();
        }

        // GET api/<BlogController>/5
        [HttpGet("{id}")]
        public async Task<BlogPost> Get(string id)
        {
            return await _elasticService.SearchBlog(id);
        }

        [HttpGet("Content/{content}")]
        public async Task<List<BlogPost>> SearchContent(string content)
        {
            return await _elasticService.SearchContent(content);
        }

        [HttpGet("Title/{title}")]
        public async Task<List<BlogPost>> SearchTitle(string title)
        {
            return await _elasticService.SearchTitle(title);
        }

        [HttpGet("Search/")]
        public async Task<List<BlogPost>> SearchBlogs(string? content, string? title)
        {
            var query = new SearchQueryDTO();
            query.Title = title;
            query.Content = content;
            return await _elasticService.SearchBlogs(query);
        }

        [HttpGet("Category/{content}")]
        public async Task<List<BlogPost>> SearchCategory(string content)
        {
            return await _elasticService.SearchCategory(content);
        }

        // POST api/<BlogController>
        [HttpPost]
        public async Task<string> PostBlog([FromBody] BlogPost blog)
        {
            return await _elasticService.InsertAsync(blog);

        }

        [HttpPut("Approve")]
        public async Task<bool> ApproveBlog(AprooveDTO id)
        {
            return await _elasticService.Approve(id);
        }

        // PUT api/<BlogController>/5
        [HttpPut]
        public async void Put( BlogPost value)
        {
            await _elasticService.UpdateBlog(value);
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _elasticService.DeleteAsync(id);
        }


    }
}
