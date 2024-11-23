using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;

namespace Explorer.Payment.Core.UseCases.Tourist;

public sealed class BundleService : BaseService<BundleDto, TourBundle>, IBundleService
{
    private readonly ICrudRepository<TourBundle> _tourBundleRepository;
    private readonly IProfileService_Internal _profileService;
    private readonly ITourService_Internal _tourService;

    public BundleService(ICrudRepository<TourBundle> tourBundleRepository, IMapper mapper, IProfileService_Internal profileService, ITourService_Internal tourService) : base(mapper)
    {
        _tourBundleRepository = tourBundleRepository;
        _profileService = profileService;
        _tourService = tourService;
    }


    public Result<BundleDto> CreateBundle(long authorId, BundleDto bundleDto)
    {
        TourBundle bundle = new(authorId, bundleDto.Name, bundleDto.Price, BundleStatus.Draft);

        List<TourDto> tours = bundleDto.Tours.Select(t => _tourService.GetById(t.TourId).Value).ToList();

        foreach (var tour in tours)
        {
            if (tour.UserId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to create a bundle with tours that do not belong to you.");
            }

            if (bundle.Tours.Any(t => t == tour.Id))
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("This tour is already in the bundle.");
            }

            bundle.Tours.Add(tour.Id);
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

            // If two or more tours are published, the bundle can be published.
            if (bundle.Tours.Count(t => _tourService.GetById(t).Value.Status == TourStatus.Published) < 2)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("At least two tours need to be published in order to publish the bundle.");
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

    public Result<PagedResult<FullBundleDto>> GetAll()
    {
        var fullBundleDtos = new List<FullBundleDto>();

        var bundles = _tourBundleRepository.GetPaged(1, int.MaxValue).Results.ToList();

        foreach (var bundle in bundles)
        {
            var id = bundle.Id;

            fullBundleDtos.Add(new FullBundleDto
            {
                Id = id,
                Name = bundle.Name,
                Price = bundle.Price,
                Status = bundle.Status,
                Tours = bundle.Tours.Select(t =>
                {
                    var tour = _tourService.GetById(t).Value;
                    return new BundleItemDto
                    {
                        TourId = tour.Id,
                        Price = tour.Price,
                        TourStatus = tour.Status,
                    };
                }).ToList()
            });
        }

        var pagedResult = new PagedResult<FullBundleDto>(fullBundleDtos, fullBundleDtos.Count);
        return pagedResult;
    }

    public Result<PagedResult<FullBundleDto>> GetMyBundles(int authorId)
    {
        var fullBundleDtos = new List<FullBundleDto>();

        var bundles = _tourBundleRepository.GetPaged(1, int.MaxValue).Results.Where(b => b.AuthorId == authorId).ToList();

        foreach (var bundle in bundles)
        {
            var id = bundle.Id;

            fullBundleDtos.Add(new FullBundleDto
            {
                Id = id,
                Name = bundle.Name,
                Price = bundle.Price,
                Status = bundle.Status,
                Tours = bundle.Tours.Select(t =>
                {
                    var tour = _tourService.GetById(t).Value;
                    return new BundleItemDto
                    {
                        TourId = tour.Id,
                        Price = tour.Price,
                        TourStatus = tour.Status,
                    };
                }).ToList()
            });
        }

        var pagedResult = new PagedResult<FullBundleDto>(fullBundleDtos, fullBundleDtos.Count);
        return pagedResult;
    }

    public Result<BundleDto> UpdateBundle(long authorId, long bundleId, BundleDto bundle)
    {
        try
        {
            TourBundle tourBundle = _tourBundleRepository.Get(bundleId);

            if (tourBundle.AuthorId != authorId)
            {
                return Result.Fail(FailureCode.Forbidden).WithError("You are not authorized to update this bundle.");
            }

            tourBundle.Name = bundle.Name;
            tourBundle.Price = bundle.Price;
            tourBundle.Status = bundle.Status ?? BundleStatus.Draft;
            tourBundle.Tours = bundle.Tours.Select(t => (int)t.TourId).ToList();

            _tourBundleRepository.Update(tourBundle);

            return MapToDto(tourBundle);
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle not found.");
        }
    }
}
