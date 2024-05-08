using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebApplication1.Controllers.Services;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        public readonly MongoService _mongoService;
        // GET: api/<GameController>

        public GameController(MongoService mongoService) { _mongoService = mongoService; }

        [HttpGet]
        public async Task<List<Game>> GetAsync()
        {
            return await _mongoService.GetAsync();
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public async Task<Game?> Get(string id)
        {
            return await _mongoService.GetAsync(id);
        }

        // POST api/<GameController>
        [HttpPost]
        public async void Post([FromBody] Game value)
        {
            value.GameId = ObjectId.GenerateNewId().ToString();
            await _mongoService.CreateAsync(value);
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
