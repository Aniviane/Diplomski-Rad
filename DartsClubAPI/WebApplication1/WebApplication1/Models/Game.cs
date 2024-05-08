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

       
    }
}
