using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
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


        public async Task<bool> CreateAsync(Game newGame)
        {
            try
            {
                await _gameCollection.InsertOneAsync(newGame);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task UpdateAsync(string id, Game updatedGame) =>
            await _gameCollection.ReplaceOneAsync(x => x.GameId == id, updatedGame);

        public async Task RemoveAsync(string id) =>
            await _gameCollection.DeleteOneAsync(x => x.GameId == id);


        public async Task<UserAveragesDTO> GetAverages(Guid playerId)
        {



            var pipeline = new BsonDocument[]
                {
                    

            // Stage 1: Match documents where the playerIds array contains the specified playerId
            new BsonDocument("$match", new BsonDocument
            {
                { "GameType", "1v1"},
                { "PersonalGames.PlayerId",  playerId}
            }),

            // Stage 2: Find index of playerId in PlayerIds array and retrieve GameScores and BullsEyes
            new BsonDocument("$unwind", "$PersonalGames"),
                       
            // Stage 3: Match Personal Games to playerId

            new BsonDocument("$match", new BsonDocument("PersonalGames.PlayerId", playerId)),

            // Stage 4: Group by any field (e.g., PlayerIdIndex) and calculate averages
                  
            new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", "$PersonalGames.PlayerId" },  // Group by PlayerId
                { "PointAverage", new BsonDocument("$avg", "$PersonalGames.Score") },
                { "BullsEyeAverage", new BsonDocument("$avg", "$PersonalGames.BullsEyes") },
                { "TripleTwentyAverage", new BsonDocument("$avg", "$PersonalGames.TripleTwenties") },
                { "WinCount", new BsonDocument("$sum",
                            new BsonDocument("$cond", new BsonArray
                            {
                                new BsonDocument("$eq", new BsonArray { "$PersonalGames.Score", 301 }),
                                1,
                                0
                            })
                        )},
                    { "LossCount", new BsonDocument("$sum",
                        new BsonDocument("$cond", new BsonArray
                        {
                            new BsonDocument("$ne", new BsonArray { "$PersonalGames.Score", 301 }),
                            1,
                            0
                        })
                    )}
            }
        )


                };


         


            var result =  _gameCollection.Aggregate<UserAveragesDTO>(pipeline).ToList().FirstOrDefault();

            if (result == null) return new UserAveragesDTO(0,0,0,0,0);

            return result;
        }

        public async Task<List<PersonalGameDTO>> FindGamesById(Guid playerId)
        {

            var games = await _gameCollection.Find(x => x.PersonalGames.Any(x => x.PlayerId == playerId)).ToListAsync();

            List<PersonalGameDTO> gamesDTO = new List<PersonalGameDTO>();

            foreach ( var game in games)
            {


                var personalGame = game.PersonalGames.Where(x => x.PlayerId == playerId).FirstOrDefault();

                if(personalGame == null) continue;

                gamesDTO.Add(new PersonalGameDTO(personalGame,game.GameId,game.Date,game.NumOfRounds,game.GameType));

            
            }

            return gamesDTO;
        }


        
    }
}
