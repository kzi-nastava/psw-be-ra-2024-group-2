using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;

namespace Explorer.Payment.Core.UseCases.Tourist;

public sealed class BundleService : CrudService<BundleDto, TourBundle>, IBundleService
{
    private readonly ICrudRepository<TourBundle> _tourBundleRepository;
    private readonly IProfileService_Internal _profileService;
    private readonly ITourService_Internal _tourService;

    public BundleService(ICrudRepository<TourBundle> tourBundleRepository, IMapper mapper, IProfileService_Internal profileService, ITourService_Internal tourService) : base(tourBundleRepository, mapper)
    {
        _tourBundleRepository = tourBundleRepository;
        _profileService = profileService;
        _tourService = tourService;
    }


    public Result<BundleDto> CreateBundle(long authorId, BundleDto bundleDto)
    {
        TourBundle bundle = new(authorId, bundleDto.Name, bundleDto.Price, BundleStatus.Draft);

        foreach (var tourDto in bundleDto.Tours)
        {
            var tour = _tourService.GetById(tourDto.TourId);

            if (tour.IsFailed)
            {
                return Result.Fail(FailureCode.NotFound).WithError("One or more tours could not be found.");
            }

            if (tour.Value.UserId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to create a bundle with tours that do not belong to you.");
            }

            if (tour.Value.Price != tourDto.Price)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("The price of the tour does not match the price in the bundle.");
            }

            if (tour.Value.Status != tourDto.TourStatus)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("The status of the tour does not match the status in the bundle.");
            }

            TourWithPrice tourWithPrice = new(tourDto.TourId, tourDto.Price, tourDto.TourStatus);
            bundle.Tours.Add(tourWithPrice);
        }

        bundle = _tourBundleRepository.Create(bundle);

        return MapToDto(bundle);
    }

    public Result<BundleDto> DeleteBundle(long authorId, long bundleId)
    {
        try
        {
            TourBundle bundle = _tourBundleRepository.Get(bundleId);

            if (bundle.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to delete this bundle.");
            }

            if (bundle.Status != BundleStatus.Published)
            {
                _tourBundleRepository.Delete(bundleId);
            }
            else
            {
                bundle.ArchiveBundle();
                _tourBundleRepository.Update(bundle);
            }

            return MapToDto(bundle);
        }
        catch
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle not found.");
        }
    }

    public Result<BundleDto> PublishBundle(long authorId, long bundleId)
    {
        try
        {
            TourBundle bundle = _tourBundleRepository.Get(bundleId);

            if (bundle.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to publish this bundle.");
            }

            if (bundle.Status == BundleStatus.Published)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("This bundle is already published.");
            }

            bundle.PublishBundle();
            _tourBundleRepository.Update(bundle);

            return MapToDto(bundle);
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle not found.");
        }
        catch (InvalidOperationException)
        {
            return Result.Fail(FailureCode.Internal).WithError("At least two tours need to be published in order to publish the bundle.");
        }
    }
    public Result<BundleDto> AddTourToBundle(long authorId, long bundleId, BundleItemDto tour)
    {
        try
        {
            TourBundle bundle = _tourBundleRepository.Get(bundleId);

            if (bundle.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to add tours to this bundle.");
            }

            var tourDto = _tourService.GetById(tour.TourId);

            if (tourDto.IsFailed)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Tour not found.");
            }

            if (tourDto.Value.UserId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to add tours that do not belong to you.");
            }

            if (tourDto.Value.Price != tour.Price)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("The price of the tour does not match the price in the bundle.");
            }

            if (tourDto.Value.Status != tour.TourStatus)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("The status of the tour does not match the status in the bundle.");
            }

            TourWithPrice tourWithPrice = new(tour.TourId, tour.Price, tour.TourStatus);
            bundle.Tours.Add(tourWithPrice);

            _tourBundleRepository.Update(bundle);

            return MapToDto(bundle);
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle not found.");
        }
    }
}
