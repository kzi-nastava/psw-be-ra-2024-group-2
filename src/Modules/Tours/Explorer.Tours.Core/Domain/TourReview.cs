using Explorer.BuildingBlocks.Core.Domain;

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

        public DateTime ReviewDate { get; set; }

        public DateTime VisitDate { get; set; }
        public double Progress { get; set; } = 0;


        public TourReview(int grade, string comment, long userId, long tourId, DateTime reviewDate, DateTime visitDate, long imageId)
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

        public TourReview(int grade, string comment, long userId, long tourId, DateTime reviewDate, DateTime visitDate)
        {
            Grade = grade;
            Comment = comment;
            UserId = userId;
            TourId = tourId;
            ReviewDate = reviewDate;
            VisitDate = visitDate;
            Validate();
        }

        public TourReview(long id)
        {
            Id = id;
        }

        public TourReview() { }

        private void Validate()
        {
            if (Grade < 1 || Grade > 5) throw new ArgumentException("Invalid grade");
        }

    }
}
