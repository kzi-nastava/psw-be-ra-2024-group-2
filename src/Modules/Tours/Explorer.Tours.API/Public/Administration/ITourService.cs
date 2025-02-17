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
        public string CancelTour(long id);
        public bool IfCancelled(long id);
        public Result<TourCreateDto> GetTourById(long tourId);
        Result<IEnumerable<TourCreateDto>> GetAllTours();
    }
}
