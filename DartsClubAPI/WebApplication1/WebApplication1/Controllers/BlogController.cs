using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Services;
using WebApplication1.Models;

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

        // GET api/<BlogController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BlogController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BlogController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
