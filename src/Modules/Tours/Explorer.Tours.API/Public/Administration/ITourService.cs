using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourService
    {
        Result<TourCreateDto> Create(TourCreateDto tour);
        Result<TourCreateDto> Update(TourCreateDto tour);
        Result Delete(long displayId);
        Result<IEnumerable<TourCreateDto>> GetGuideTours(long guideId);
        bool Exists(long displayId);
        public Result CancelTour(long id);
        public bool IfCancelled(long id);
        public Result<TourCreateDto> GetTourById(long tourId);
        Result<IEnumerable<TourCreateDto>> GetAllTours();
        public Result BuyTours(long userId, List<long> tourIds);

        /*
        public void SendTestEmail();
        public void SendTourReminderEmails();
        Task SendReportEmail(string emailAddress, string reportContent);  // Ensure correct method signature

        */

        public Result CreateTourRate(TourRateDto tourRateDto);

        public Result<TourRateDto>? GetTourRateByUser(long userId, long tourId);

        public bool CheckIfTourGuide(long guideId, long tourId);

        public Result<List<TourRateDto>> GetTourRates(long tourId);

        // This is reporting part
        public int GetTourRateCount(long tourId);

        public Result<TourCreateDto> GetBestRatedTourByGuideID(long guideID);

        public Result<TourCreateDto> GetLowestRatedTourByGuideID(long guideID);

        public GuideReportDto GetMonthlyGuideTourReport(long guideID, int year, int month);

        public List<long> GetAllGuidesWithTourInLastMonth();

        public List<long> GetAllGuides();


    }
}
