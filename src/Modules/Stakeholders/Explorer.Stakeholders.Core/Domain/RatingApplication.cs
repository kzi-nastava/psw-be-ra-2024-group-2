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
        public long PersonId { get; set; }

        public RatingApplication(int grade, string? comment, DateTime ratingTime, long personId)
        {
            Grade = grade;
            Comment = comment;
            RatingTime = ratingTime;
            PersonId = personId;
            Validate();
        }
        private void Validate()
        {
            if (PersonId == 0) throw new ArgumentException("Invalid PersonId");
            if (Grade > 5 || Grade < 1) throw new ArgumentException("Invalid Grade");
            if (RatingTime > DateTime.Now) throw new ArgumentException("Invalid RatingTime");
        }


    }
}
