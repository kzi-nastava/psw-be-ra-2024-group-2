using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        bool Exists(long id);

        Tour Create(Tour tour);

        Tour Update(Tour tour);

        IEnumerable<Tour> GetGuideTours(long guideId);

        IEnumerable<Tour> GetAllTours();

        Tour? GetTourByID(long tourId);

        public KeyPoint CreateKeyPoint(KeyPoint keyPoint);

        public KeyPoint DeleteKeyPoint(KeyPoint keyPoint);

        public KeyPoint UpdateKeyPoint(KeyPoint keyPoint);

        public UserTourPurchase CreatePurchase(UserTourPurchase purchase);

        public int GetTourPurchaseCount(long tourId);

        public List<Tour> GetToursByIds(List<long> tourIds);

        public void CancelTourPurchases(long tourId);

        public void CancelTour(long tourId);

        public List<String> GetTourReminderEmails(long tourId);

        public List<Tour> GetReminderTours();

        public TourRate CreateTourRate(TourRateDto tourRateDto);

        public List<TourRate> GetTourRates(long tourId);

        public double GetAverageTourRate(long tourId);

        public TourRate? GetTourRateByUser(long userId, long tourId);

        public List<Tour> GetLastMonthGuideTours(long guideID);

        public List<long> GetAllGuidesWithTourInLastMonth();

        public List<long> GetAllGuides();

        public bool CheckIfTourGuide(long guideId, long tourId);
    }

}
