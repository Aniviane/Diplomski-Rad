using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameId { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; } = DateTime.Now!;

        [BsonElement("GameType")]
        public string GameType { get; set; }

       

        [BsonElement("NumOfRounds")]
        public int NumOfRounds { get; set; }


        [BsonElement("PersonalGames")]
        public List<PersonalGame> PersonalGames { get; set; }



        public Game() { }

        public Game(DateTime date, List<int> gameScores, string gameType, List<PersonalGame> personalGames, int numOfRounds)
        {
            Date = date;
            PersonalGames = personalGames;
            GameType = gameType;
            NumOfRounds = numOfRounds;
        }

    }
}
