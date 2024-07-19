using MongoDB.Bson;

namespace WebApplication1.Models.DTO_s
{
    public class UserAveragesDTO
    {
        public UserAveragesDTO(double pointAverage, double bullsEyeAverage, double tripleTwentyAverage, double winCount, double lossCount)
        {
            PointAverage = Math.Round(pointAverage,2);
            BullsEyeAverage = Math.Round(bullsEyeAverage,2);
            TripleTwentyAverage = Math.Round(tripleTwentyAverage,2);
            WinCount = winCount;
            LossCount = lossCount;
        }
        public Guid? Id { get; set; }
        public double PointAverage { get; set; }
        public double BullsEyeAverage { get; set; }

        public double TripleTwentyAverage { get; set; }

        public double WinCount { get; set; }

        public double LossCount { get; set; }
    }
}
