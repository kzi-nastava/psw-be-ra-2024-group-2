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


    }
}
