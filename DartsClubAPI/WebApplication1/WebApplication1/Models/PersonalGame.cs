using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace WebApplication1.Models
{
    public class PersonalGame
    {
        public PersonalGame(Guid playerId, int score, int tripleTwenties, int bullsEyes)
        {
            PlayerId = playerId;
            Score = score;
            TripleTwenties = tripleTwenties;
            BullsEyes = bullsEyes;
        }




        [BsonElement("PlayerId")]
        public Guid PlayerId { get; set; }

        [BsonElement("Score")]
        public int Score { get; set; }

        [BsonElement("TripleTwenties")]
        public int TripleTwenties { get; set; }

        [BsonElement("BullsEyes")]
        public int BullsEyes { get; set; }


    }
}
