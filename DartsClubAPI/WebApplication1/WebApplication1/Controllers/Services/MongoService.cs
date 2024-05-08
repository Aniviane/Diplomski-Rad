using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<Game>? _gameCollection;

        public MongoService(
       IOptions<MongoConfig> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _gameCollection = mongoDatabase.GetCollection<Game>(
                settings.Value.CollectionName);
        }

        public async Task<List<Game>> GetAsync() =>
       await _gameCollection.Find(_ => true).ToListAsync();

        public async Task<Game?> GetAsync(string id) =>
            await _gameCollection.Find(x => x.GameId == id).FirstOrDefaultAsync();


        public async Task CreateAsync(Game newGame) =>
      await _gameCollection.InsertOneAsync(newGame);

        public async Task UpdateAsync(string id, Game updatedGame) =>
            await _gameCollection.ReplaceOneAsync(x => x.GameId == id, updatedGame);

        public async Task RemoveAsync(string id) =>
            await _gameCollection.DeleteOneAsync(x => x.GameId == id);

    }
}
