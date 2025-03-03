using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Tour : Entity
    {
        public string Name { get; private set; }  // 'naziv ture'
        public string Description { get; private set; }  // 'opis'
        public int Difficulty { get; private set; }  // 'težinu ture' (difficulty of the tour)
        public TourCategory Category { get; private set; }  // 'kategoriju ture' (category of the tour)
        public decimal Price { get; private set; }  // 'cenu' (price)
        public DateTime Date { get; private set; }  // 'datum održavanja ture' (tour date)
        public long GuideId { get; protected set; }
        public TourStatus Status { get; private set; }
        public List<KeyPoint> KeyPoints { get; private set; }

        public Tour(string name, string description, int difficulty, TourCategory category, decimal price, DateTime date, long guideId)
        {
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Category = category;
            Price = price;
            Date = date;
            GuideId = guideId;
            Status = TourStatus.Draft;
            KeyPoints = new List<KeyPoint>();
            Validate();
        }

        public void UpdateTourStatus(TourStatus newStatus)
        {
            this.Status = newStatus;
        }

        public void UpdateTour(Tour updatedTour)
        {
            // Ensure that the current tour is not null
            if (updatedTour == null)
            {
                throw new ArgumentNullException(nameof(updatedTour), "The tour to update cannot be null.");
            }

            // Update the fields from the input Tour
            Name = updatedTour.Name;
            Description = updatedTour.Description;
            Difficulty = updatedTour.Difficulty;
            Category = updatedTour.Category;
            Price = updatedTour.Price;
            Date = updatedTour.Date;
            //GuideId = updatedTour.GuideId;

            // Optional: Update the status if needed, e.g., from Draft to Published
            // Status = updatedTour.Status;  // Only if status needs to be updated

            // Update KeyPoints if necessary
            //KeyPoints = updatedTour.KeyPoints != null ? new List<KeyPoint>(updatedTour.KeyPoints) : new List<KeyPoint>();

            // Validate if needed (depending on your business logic)
            Validate();
        }


        public void AddKeyPoint(KeyPoint keyPoint)
        {
            KeyPoints.Add(keyPoint);
        }

        //public setTourStatus(TourStatus newStatus)

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid tour name");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid description");
            if (Difficulty < 1 || Difficulty > 5) throw new ArgumentException("Invalid difficulty, must be between 1 and 5");
            if (Price <= 0) throw new ArgumentException("Invalid price");
        }

        public string GetCategoryName()
        {
            return Category.ToString().ToLower();
        }
    }

    public enum TourCategory
    {
        Hiking,
        Cultural,
        Adventure,
        Nature,
        CityTour
    }

    public enum TourStatus
    {
        Draft,
        Published,
        Cancelled,
    }
}
