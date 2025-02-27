﻿using MongoDB.Bson.Serialization.Attributes;
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

        [BsonElement("GameScores")]
        public List<int> GameScores { get; set; }

        [BsonElement("PlayerIds")]
        public List<Guid> PlayerIds { get; set; }

        [BsonElement("TripleTwentys")]
        public List<int> TripleTwentys { get; set; }

        [BsonElement("BullsEyes")]
        public List<int> BullsEyes { get; set; }

        [BsonElement("NumOfRounds")]
        public int NumOfRounds { get; set; }



        public Game() { }

        public Game(DateTime date, List<int> gameScores, string gameType, List<Guid> playerIds, List<int> tripleTwentys, List<int> bullsEyes, int numOfRounds)
        {
            Date = date;
            GameScores = gameScores;
            GameType = gameType;
            PlayerIds = playerIds;
            TripleTwentys = tripleTwentys;
            BullsEyes = bullsEyes;
            NumOfRounds = numOfRounds;
        }

    }
}
