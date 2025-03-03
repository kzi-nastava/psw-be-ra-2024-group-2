using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourDatabaseRepository : ITourRepository
    {
        private readonly ToursContext _dbContext;

        public TourDatabaseRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Check if a tour exists by its name
        public bool Exists(long id)
        {
            return _dbContext.Tours.Any(tour => tour.Id == id);
        }

        public List<long> GetAllGuidesWithTourInLastMonth()
        {
            // Get the current date and ensure it is in UTC
            var currentTime = DateTime.UtcNow;

            // First day of last month (UTC)
            var lastMonthStartDate = new DateTime(currentTime.Year, currentTime.Month, 1).AddMonths(-1).ToUniversalTime();

            // Last day of last month (UTC)
            var lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1).ToUniversalTime();

            // Fetch the guide IDs for tours in the last month
            var guideIds = _dbContext.Tours
                                     .Where(tour => tour.GuideId != null &&
                                                    tour.Date >= lastMonthStartDate &&
                                                    tour.Date <= lastMonthEndDate) // Ensure the TourDate is in the last month
                                     .GroupBy(tour => tour.GuideId) // Group by GuideId to remove duplicates
                                     .Select(group => group.Key) // Select the GuideId (group.Key)
                                     .ToList();

            return guideIds;
        }



        public List<long> GetAllGuides()
        {
            // Fetch unique guide IDs by grouping by GuideId and selecting the first entry from each group
            var guideIds = _dbContext.Tours
                                     .Where(tour => tour.GuideId != null) // Ensure GuideId is not null
                                     .GroupBy(tour => tour.GuideId) // Group by GuideId to remove duplicates
                                     .Select(group => group.Key) // Select the GuideId (group.Key)
                                     .ToList();

            return guideIds;
        }


        // Create a new tour
        public Tour Create(Tour tour)
        {
            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();
            return tour;
        }

        public KeyPoint CreateKeyPoint(KeyPoint keyPoint)
        {
            _dbContext.KeyPoints.Add(keyPoint);
            _dbContext.SaveChanges();
            return keyPoint;
        }

        public List<TourRate> GetTourRates(long tourId)
        {
            return _dbContext.TourRates
                             .Where(tr => tr.TourId == tourId)
                             .ToList();
        }

        public bool CheckIfTourGuide(long guideId, long tourId)
        {
            return _dbContext.Tours.Any(t => t.Id == tourId && t.GuideId == guideId);
        }



        public double GetAverageTourRate(long tourId)
        {
            var averageRating = _dbContext.TourRates
                                          .Where(tr => tr.TourId == tourId)
                                          .Select(tr => (double?)tr.Rating) // Cast to nullable to handle empty results
                                          .Average();

            return averageRating ?? 0; // Return 0 if there are no ratings
        }

        public TourRate? GetTourRateByUser(long userId, long tourId)
        {
            var tourRate = _dbContext.TourRates
                                     .FirstOrDefault(tr => tr.TouristId == userId && tr.TourId == tourId);

            return tourRate ?? null; // Explicitly returning null if not found
        }

        public List<Tour> GetLastMonthGuideTours(long guideID)
        {
            // Get the current date and time in UTC
            var currentDate = DateTime.UtcNow;

            // First day of the last month in UTC
            var startOfLastMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1).ToUniversalTime();

            // Last day of the last month in UTC
            var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1).ToUniversalTime();

            // Query to get tours conducted by the guide during the last month
            var tours = _dbContext.Tours
                .Where(t => t.GuideId == guideID &&
                            t.Date >= startOfLastMonth &&
                            t.Date <= endOfLastMonth)
                .ToList();

            return tours;
        }


        public int GetTourPurchaseCount(long tourId)
        {
            // Validate the input (optional but good practice)
            if (tourId <= 0)
            {
                return -1;
            }

            // Query the Purchases table to count the number of purchases for the specific tourId
            var purchaseCount = _dbContext.Purchases
                .Where(p => p.TourId == tourId)
                .Count();

            return purchaseCount;
        }

        public UserTourPurchase CreatePurchase(UserTourPurchase purchase)
        {
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();
            return purchase;
        }

        public List<string> GetTourReminderEmails(long tourId)
        {
            var emails = _dbContext.Purchases
                .Where(p => p.TourId == tourId) // Filter by the given tourId
                .Select(p => p.UserEmail)       // Select the email field
                .ToList();                      // Convert the result to a list

            return emails;
        }

        public List<Tour> GetReminderTours()
        {
            var now = DateTime.UtcNow;
            var startDate = now.AddDays(2).Date; // Start of the day 2 days from now
            var endDate = startDate.AddDays(1).AddTicks(-1); // End of the same day

            var reminderTours = _dbContext.Tours
                .Where(t => t.Date >= startDate && t.Date <= endDate) // Only tours on that exact day
                .ToList();

            return reminderTours;
        }



        public List<Tour> GetToursByIds(List<long> tourIds)
        {          
            return _dbContext.Tours
                            .Where(t => tourIds.Contains(t.Id))
                            .ToList();
        }

        public TourRate CreateTourRate(TourRateDto tourRateDto)
        {
            TourRate tourRate = new TourRate(tourRateDto);
            _dbContext.TourRates.Add(tourRate);

            _dbContext.SaveChanges();

            return tourRate;
        }


        public UserTourPurchase CancelPurchase(UserTourPurchase purchase)
        {
            // Find the purchase in the database by userId and tourId
            var existingPurchase = _dbContext.Purchases
                .FirstOrDefault(p => p.UserId == purchase.UserId && p.TourId == purchase.TourId);

            if (existingPurchase == null)
            {
                // Handle the case when the purchase is not found
                // You might want to throw an exception or return null
                throw new Exception("Purchase not found.");
            }

            // Update the status of the existing purchase to 'Cancelled' (status = 2)
            existingPurchase.UpdateStatus(PurchaseStatus.Cancelled);

            // Save the changes to the database
            _dbContext.SaveChanges();

            return existingPurchase;
        }

        public void CancelTour(long tourId)
        {
            // Find the tour by ID
            var tour = _dbContext.Tours.FirstOrDefault(t => t.Id == tourId);

            if (tour == null)
            {
                throw new Exception("Tour not found.");
            }

            // Change the tour status to Cancelled (assuming 2 represents Cancelled)
            tour.UpdateTourStatus(TourStatus.Cancelled);

            // Save changes to update the tour status
            _dbContext.SaveChanges();

            // Cancel all purchases associated with this tour
            CancelTourPurchases(tourId);
        }


        public void CancelTourPurchases(long tourId)
        {
            // Find all purchases with the given tourId
            var purchases = _dbContext.Purchases.Where(p => p.TourId == tourId).ToList();

            if (!purchases.Any())
            {
                return;
            }

            // Update the status of each purchase to 'Cancelled' (assuming 2 is the value for Cancelled)
            foreach (var purchase in purchases)
            {
                purchase.UpdateStatus(PurchaseStatus.Cancelled);
            }

            // Save the changes to the database
            _dbContext.SaveChanges();
        }

        public KeyPoint UpdateKeyPoint(KeyPoint keyPoint)
        {
            var existingKeyPoint = _dbContext.KeyPoints.Find(keyPoint.Id);
            if (existingKeyPoint == null)
            {
                throw new ArgumentException($"KeyPoint with ID {keyPoint.Id} not found.");
            }

            // Update properties
            existingKeyPoint.UpdateKeyPoint(keyPoint.Latitude, keyPoint.Longitude, keyPoint.Name, keyPoint.Description);

            _dbContext.SaveChanges();
            return existingKeyPoint;
        }

        public KeyPoint DeleteKeyPoint(KeyPoint deleteKeyPoint)
        {
            var keyPoint = _dbContext.KeyPoints.Find(deleteKeyPoint.Id);
            if (keyPoint == null)
            {
                throw new ArgumentException($"KeyPoint with ID {deleteKeyPoint.Id} not found.");
            }

            _dbContext.KeyPoints.Remove(keyPoint);
            _dbContext.SaveChanges();

            return keyPoint;
        }


        public Tour Update(Tour tour)
        {
            // Find the existing tour in the database by ID
            var existingTour = _dbContext.Tours.FirstOrDefault(t => t.Id == tour.Id);

            // If the tour doesn't exist, throw an exception or return an error
            if (existingTour == null)
            {
                throw new ArgumentException($"Tour with ID {tour.Id} not found.");
            }

            existingTour.UpdateTour(tour);

            // Save the changes to the database
            _dbContext.SaveChanges();

            // Return the updated tour
            return existingTour;
        }


        // Get all tours associated with a specific guide (by guideId)
        public IEnumerable<Tour> GetGuideTours(long guideId)
        {
            return _dbContext.Tours
                .Where(tour => tour.GuideId == guideId)
                .Include(tour => tour.KeyPoints) // This will load related KeyPoints for each Tour
                .ToList();
        }

        public IEnumerable<Tour> GetAllTours()
        {
            return _dbContext.Tours.ToList();                
        }


        // Get a tour by its display ID
        public Tour? GetTourByID(long tourId)
        {
            return _dbContext.Tours
                             .Where(tour => tour.Id == tourId)
                             .Include(tour => tour.KeyPoints)
                             .FirstOrDefault();
        }

    }
}
