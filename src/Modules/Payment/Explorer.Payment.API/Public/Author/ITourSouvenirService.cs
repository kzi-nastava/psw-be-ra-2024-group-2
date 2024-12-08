using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using FluentResults;

namespace Explorer.Payment.API.Public.Author;
public interface ITourSouvenirService
{
    Result<TourSouvenirDto> CreateSouvenir(int authorId, TourSouvenirDto souvenir);
    Result<TourSouvenirDto> DeleteSouvenir(int authorId, long id);
    Result<PagedResult<TourSouvenirDto>> GetMySouvenirs(int authorId);
    Result<TourSouvenirDto> PublishSouvenir(int authorId, long id);
    Result<TourSouvenirDto> UpdateSouvenir(int authorId, TourSouvenirDto souvenir);
    Result<TourSouvenirDto> UpdateSouvenirCount(int authorId, long id, int count);
}
