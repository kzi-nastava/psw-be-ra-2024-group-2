using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public long UserId { get; set; }

        public long TourId { get; set; }

        public long? ImageId { get; set; }

        public Image? Image { get; set; }

        public DateOnly ReviewDate { get; set; }

        public DateOnly VisitDate { get; set; }


        public TourReview(int grade, string comment, long userId, long tourId, DateOnly reviewDate, DateOnly visitDate, long imageId)
        {
            Grade = grade;
            Comment = comment;
            UserId = userId;  
            TourId = tourId;
            ReviewDate = reviewDate;
            VisitDate = visitDate;
            ImageId = imageId;
            Validate();
        }

        public TourReview(int grade, string comment, long userId, long tourId, DateOnly reviewDate, DateOnly visitDate)
        {
            Grade = grade;
            Comment = comment;
            UserId = userId;
            TourId = tourId;
            ReviewDate = reviewDate;
            VisitDate = visitDate;
            Validate();
        }

        public TourReview() { }

        private void Validate()
        {
            if (Grade < 1 || Grade > 5) throw new ArgumentException("Invalid grade");
        }

    }
}
