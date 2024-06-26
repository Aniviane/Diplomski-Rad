using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebApplication1.Controllers.Services;
using WebApplication1.Models;
using WebApplication1.Models.DTO_s;

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

        [HttpGet("Averages/{id}")]
        public async Task<UserAveragesDTO> GetMyAverages(Guid id)
        {
            return await _mongoService.GetAverages(id);
        }

        [HttpGet("PlayerId/{id}")]
        public async Task<List<PersonalGameDTO>> GetGamesById(Guid id)
        {
            return await _mongoService.FindGamesById(id);
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
        public async void Put(string id, [FromBody] Game value)
        {
            await _mongoService.UpdateAsync(id, value);
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
