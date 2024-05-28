using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Game
    {
        public Game(DateTime date, int aScore, int bScore, string gameType, List<Guid> playerIds, int numOfRounds)
        {
            Date = date;
            AScore = aScore;
            BScore = bScore;
            GameType = gameType;
            PlayerIds = playerIds;
            NumOfRounds = numOfRounds;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameId { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; } = DateTime.Now!;

        [BsonElement("AScore")]
        public int AScore { get; set; }

        [BsonElement("BScore")]
        public int BScore { get; set; }


        [BsonElement("GameType")]
        public string GameType { get; set; }

        [BsonElement("PlayerIds")]
        public List<Guid> PlayerIds { get; set; }

        [BsonElement("NumOfRounds")]
        public int NumOfRounds {  get; set; }
    }
}
