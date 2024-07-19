using MongoDB.Bson;

namespace WebApplication1.Models.DTO_s
{
    public class PersonalGameDTO
    {
        public PersonalGameDTO(string gameId, int score, int tripleTwenties, int bullsEyes, DateTime gameDate, int rounds, string gameType)
        {
            GameId = gameId;
            Score = score;
            TripleTwenties = tripleTwenties;
            BullsEyes = bullsEyes;
            GameDate = gameDate;
            Rounds = rounds;
            GameType = gameType;
        }

        public DateTime Date { get; set; }

        public ObjectId Id { get; set; }

        public string GameId { get; set; }


        public string GameType { get; set;}

        public int Score { get; set; }

        public int TripleTwenties { get; set; }

        public int BullsEyes { get; set;}

        public DateTime GameDate { get; set; }

        public int Rounds { get; set; }

    }
}

