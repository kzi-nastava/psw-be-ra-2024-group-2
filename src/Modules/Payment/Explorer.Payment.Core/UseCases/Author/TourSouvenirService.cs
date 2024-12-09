using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.Core.Domain;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;

namespace Explorer.Payment.Core.UseCases.Author;

public sealed class TourSouvenirService : BaseService<TourSouvenirDto, TourSouvenir>, ITourSouvenirService
{
    private readonly ICrudRepository<TourSouvenir> _tourSouvenirRepository;
    private readonly ITourService_Internal _tourService_Internal;
    private readonly IImageRepository _imageRepository;

    public TourSouvenirService(ICrudRepository<TourSouvenir> tourSouvenirRepository, ITourService_Internal tourService_Internal, IImageRepository imageRepository, IMapper _mapper)
        : base(_mapper)
    {
        _tourSouvenirRepository = tourSouvenirRepository;
        _tourService_Internal = tourService_Internal;
        _imageRepository = imageRepository;
    }

    public Result<TourSouvenirDto> CreateSouvenir(int authorId, TourSouvenirDto souvenirDto)
    {
        var tourResult = _tourService_Internal.GetById(souvenirDto.TourId);

        if(tourResult.IsFailed)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Tour not found");
        }

        var tour = tourResult.Value;

        if(tour.UserId != authorId)
        {
            return Result.Fail(FailureCode.Forbidden).WithError("You are not the author of this tour");
        }

        if(tour.Status != TourStatus.Published)
        {
            return Result.Fail(FailureCode.Forbidden).WithError("You can only create souvenirs for published tours");
        }

        try
        {
            var souvenir = new TourSouvenir(souvenirDto.Name, souvenirDto.Price, souvenirDto.Description, souvenirDto.Count, souvenirDto.SouvenirStatus ?? SouvenirStatus.Draft, 
                new Image(souvenirDto.ImageDto.Data, souvenirDto.ImageDto.UploadedAt, souvenirDto.ImageDto.MimeType), souvenirDto.TourId, authorId);

            var result = _tourSouvenirRepository.Create(souvenir);

            return MapToDto(result);
        } catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while creating the souvenir");
        }
    }

    public Result<TourSouvenirDto> DeleteSouvenir(int authorId, long id)
    {
        try
        {
            var souvenir = _tourSouvenirRepository.Get(id);

            if (souvenir.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not the author of this souvenir");
            }

            if(souvenir.SouvenirStatus == SouvenirStatus.Published)
            {
                souvenir.Archive();

                _tourSouvenirRepository.Update(souvenir);
            } else
            {
                _tourSouvenirRepository.Delete(souvenir.Id);
            }

            return MapToDto(souvenir);
        } catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while deleting the souvenir");
        }
    }

    public Result<PagedResult<TourSouvenirDto>> GetAll()
    {
        try
        {
            var souvenirs = _tourSouvenirRepository.GetPaged(1, int.MaxValue).Results;

            var souvenirsDto = souvenirs.Select(MapToDto).ToList();

            var pagedResult = new PagedResult<TourSouvenirDto>(souvenirsDto, souvenirsDto.Count);

            return pagedResult;
        }
        catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while getting the souvenirs");
        }
    }

    public Result<PagedResult<TourSouvenirDto>> GetMySouvenirs(int userId)
    {
        try
        {
            var souvenirs = _tourSouvenirRepository.GetPaged(1, int.MaxValue).Results.Where(s => s.AuthorId == userId);

            var souvenirsDto = souvenirs.Select(MapToDto).ToList();

            var pagedResult = new PagedResult<TourSouvenirDto>(souvenirsDto, souvenirsDto.Count);

            return pagedResult;
        }
        catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while getting the souvenirs");
        }
    }

    public Result<TourSouvenirDto> PublishSouvenir(int authorId, long id)
    {
        try
        {
            var souvenir = _tourSouvenirRepository.Get(id);

            if (souvenir.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not the author of this souvenir");
            }

            if (souvenir.SouvenirStatus == SouvenirStatus.Published)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("This souvenir is already published");
            }

            souvenir.Publish();

            _tourSouvenirRepository.Update(souvenir);

            return MapToDto(souvenir);
        }
        catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while publishing the souvenir");
        }
    }

    public Result<TourSouvenirDto> UpdateSouvenir(int authorId, TourSouvenirDto souvenir)
    {
        try
        {
            var souvenirToUpdate = _tourSouvenirRepository.Get(souvenir.Id ?? 0);

            if (souvenirToUpdate.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not the author of this souvenir");
            }

            if (souvenirToUpdate.SouvenirStatus == SouvenirStatus.Published)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You cannot update a published souvenir");
            }

            souvenirToUpdate.Update(souvenir.Name, souvenir.Price, souvenir.Description, souvenir.Count, souvenir.SouvenirStatus ?? souvenirToUpdate.SouvenirStatus, souvenirToUpdate.Image.Id);

            _tourSouvenirRepository.Update(souvenirToUpdate);

            return MapToDto(souvenirToUpdate);
        }
        catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while updating the souvenir");
        }
    }

    public Result<TourSouvenirDto> UpdateSouvenirCount(int authorId, long id, int count)
    {
        try
        {
            var souvenir = _tourSouvenirRepository.Get(id);

            if (souvenir.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not the author of this souvenir");
            }

            souvenir.UpdateCount(count);

            _tourSouvenirRepository.Update(souvenir);

            return MapToDto(souvenir);
        }
        catch
        {
            return Result.Fail(FailureCode.Internal).WithError("An error occurred while updating the souvenir count");
        }
    }
}
