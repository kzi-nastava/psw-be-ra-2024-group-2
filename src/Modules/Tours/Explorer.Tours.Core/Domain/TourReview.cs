using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourReview : Entity
    {
        public int Grade {  get; set; }

        public string Comment { get; set; }

        public User User { get; set; }

        public DateOnly ReviewDate { get; set; }

        public DateOnly VisitDate { get; set; }

        //public Tour tour { get; set; } waiting for tour implementation

        public TourReview(int grade, string comment, User user, DateOnly reviewDate, DateOnly visitDate)
        {
            Grade = grade;
            Comment = comment;
            User = user;    
            ReviewDate = reviewDate;
            VisitDate = visitDate;
            Validate();
        }

        private void Validate()
        {
            if (Grade < 1 || Grade > 5) throw new ArgumentException("Invalid grade");
            if (User is null) throw new ArgumentException("Invalid user");
        }

    }
}
