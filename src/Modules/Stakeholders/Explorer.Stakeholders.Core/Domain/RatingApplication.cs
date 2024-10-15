using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class RatingApplication : Entity
    {
        public int Grade { get; set; }
        public string? Comment { get; set; }
        public DateTime RatingTime { get; set; }
        public long UserId { get; set; }

        public RatingApplication() { }
        public RatingApplication(int grade, string? comment, DateTime ratingTime, long userId)
        {
            Grade = grade;
            Comment = comment;
            RatingTime = ratingTime;
            UserId = userId;
            Validate();
        }
        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid PersonId");
            if (Grade > 5 || Grade < 1) throw new ArgumentException("Invalid Grade");
            if (RatingTime > DateTime.Now) throw new ArgumentException("Invalid RatingTime");
        }


    }
}
