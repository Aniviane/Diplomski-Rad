using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplication1.Models;
using WebApplication1.Models.DTO_s;

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


        public async Task<UserAveragesDTO> GetAverages(Guid id)
        {

            var games = await _gameCollection.Find(x => x.PlayerIds.Contains(id)).ToListAsync();

            if (games == null || games.Count == 0)
                return new UserAveragesDTO(0, 0, 0, 0, 0);


            int pointSum = 0;

            int ttSum = 0;

            int beSum = 0;

            int winCount = 0;


            foreach (var game in games)
            {

                if (!ValidGame(game)) continue;

                var UserIndex = game.PlayerIds.FindIndex(g => g.Equals(id));

                var score = game.GameScores[UserIndex];

                pointSum += score;
                ttSum += game.TripleTwentys[UserIndex];
                beSum += game.BullsEyes[UserIndex];

                if (score == 301)
                    winCount++;

            }

            double gameCount = games.Count;

            return new UserAveragesDTO(pointSum / gameCount, beSum / gameCount, ttSum / gameCount, winCount, gameCount - winCount);
        }

        public async Task<List<PersonalGameDTO>> FindGamesById(Guid id)
        {

            var games = await _gameCollection.Find(x => x.PlayerIds.Contains(id)).ToListAsync();

            List<PersonalGameDTO> gamesDTO = new List<PersonalGameDTO>();

            foreach ( var game in games)
            {

                if (!ValidGame(game)) continue;

                var UserIndex = game.PlayerIds.FindIndex(g => g.Equals(id));

                var bullseyes = game.BullsEyes[UserIndex];
                var tripleTwenties = game.TripleTwentys[UserIndex];
                var score = game.GameScores[UserIndex];
                var gameDate = game.Date;
                gamesDTO.Add(new PersonalGameDTO(game.GameId, score, tripleTwenties, bullseyes, gameDate,game.NumOfRounds, game.GameType));
            }

            return gamesDTO;
        }


        private bool ValidGame(Game game)
        {

            var userCount = game.PlayerIds.Count;

            if (game.PlayerIds.Count == userCount &&
                game.GameScores.Count == userCount &&
                game.BullsEyes.Count == userCount &&
                game.TripleTwentys.Count == userCount) return true;

            return false;
                
        }
    }
}
