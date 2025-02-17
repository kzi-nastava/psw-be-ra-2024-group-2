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
