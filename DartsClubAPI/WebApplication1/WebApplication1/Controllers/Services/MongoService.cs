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


        public async Task<UserAveragesDTO> GetAverages(Guid id)
        {

            var games = await _gameCollection.Find(x => x.PlayerIds.Contains(id)).ToListAsync();

            var pipeline = new BsonDocument[]
                {
                    
                    // Stage 1: Match documents where the playerIds array contains the specified playerId
                    new BsonDocument("$match",
                        new BsonDocument("PlayerIds", id)
                    ),

                    // Stage 2: Find index of playerId in PlayerIds array and retrieve GameScores and BullsEyes
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            {"PlayersId", id },
                           
                            { "GameScore",
                                new BsonDocument("$arrayElemAt", new BsonArray { "$GameScores",
                                    new BsonDocument("$indexOfArray", new BsonArray { "$PlayerIds", id}) })
                            },
                            { "BullsEye",
                                new BsonDocument("$arrayElemAt", new BsonArray { "$BullsEyes",
                                    new BsonDocument("$indexOfArray", new BsonArray { "$PlayerIds", id }) })
                            },
                            { "TripleTwentys",
                                new BsonDocument("$arrayElemAt", new BsonArray { "$TripleTwentys",
                                    new BsonDocument("$indexOfArray", new BsonArray { "$PlayerIds", id }) })
                            }
                        }
                    ),

                    // Stage 3: Group by any field (e.g., PlayerIdIndex) and calculate averages
                  
                    new BsonDocument("$group",
                    new BsonDocument
                    {
                        { "_id", "$PlayersId" },  // Group by PlayerId
                        { "PointAverage", new BsonDocument("$avg", "$GameScore") },
                        { "BullsEyeAverage", new BsonDocument("$avg", "$BullsEye") },
                        { "TripleTwentyAverage", new BsonDocument("$avg", "$TripleTwentys") },
                        { "WinCount", new BsonDocument("$sum",
                                    new BsonDocument("$cond", new BsonArray
                                    {
                                        new BsonDocument("$eq", new BsonArray { "$GameScore", 301 }),
                                        1,
                                        0
                                    })
                                )},
                            { "LossCount", new BsonDocument("$sum",
                                new BsonDocument("$cond", new BsonArray
                                {
                                    new BsonDocument("$ne", new BsonArray { "$GameScore", 301 }),
                                    1,
                                    0
                                })
                            )}
                    }
                )


                };


         


            var result = _gameCollection.Aggregate<UserAveragesDTO>(pipeline).ToList().FirstOrDefault();

            if (result == null) return new UserAveragesDTO(0,0,0,0,0);

            return result;
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
