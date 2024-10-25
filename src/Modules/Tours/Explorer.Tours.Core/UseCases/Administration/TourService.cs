using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ICrudRepository<Equipment> _equipmentRepository;
        private readonly ICrudRepository<Checkpoint> _checkpointRepository;

        public TourService(ICrudRepository<Tour> tourRepository, ICrudRepository<Equipment> equipmentRepository, ICrudRepository<Checkpoint> checkpointRepository, IMapper mapper) : base(tourRepository, mapper)
        {
            _tourRepository = tourRepository;
            _equipmentRepository = equipmentRepository;
            _checkpointRepository = checkpointRepository;
        }

        public Result<TourDto> UpdateTour(TourDto tourDto, long userId)
        {
            try
            {
                if (tourDto.UserId != userId)
                    return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add equipment to this tour");

                Tour tour = _tourRepository.Get(tourDto.Id);

                tour.Equipment.Clear();

                foreach (var elementId in tourDto.Equipment)
                {
                    var newEquipment = _equipmentRepository.Get(elementId);
                    tour.Equipment.Add(newEquipment);
                }

                _tourRepository.Update(tour);
                return MapToDto(tour);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Tour or equipment doesn't exist !");
            }
        }
        public Result<TourDto> UpdateTourCheckpoints(TourDto tourDto, long userId)
        {
            try
            {
                if (tourDto.UserId != userId)
                    return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add checkpoints");

                Tour tour = _tourRepository.Get(tourDto.Id);


                foreach (var elementId in tourDto.Checkpoints)
                {
                    var newCheckpoint = _checkpointRepository.Get(elementId);
                    tour.Checkpoints.Add(newCheckpoint);
                }

                _tourRepository.Update(tour);
                return MapToDto(tour);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Tour or checkpoint doesn't exist !");
            }
        }

        public Result<TourDto> CreateTour(TourDto dto, int userId)
        {
            dto.UserId = userId;

            Tour tour = new(dto.UserId, 
                dto.Name, 
                dto.Description,
                dto.Difficulty,
                dto.Tag, 
                dto.Status, 
                dto.Price);

            tour.TourDurationByTransports.AddRange(
                dto.TourDurationByTransportDtos?.Select(
                    t => new TourDurationByTransport(t.Transport, t.Duration)).ToList() 
                        ?? new List<TourDurationByTransport>());

            Validate(dto);

            var result = _tourRepository.Create(tour);

            return MapToDto(result);
        }

        private static void Validate(TourDto dto)
        {
            if (dto.Price != 0) throw new ArgumentException("Price must be 0");
            if (dto.Status != TourStatus.Draft) throw new ArgumentException("Invalid Status");
        }

        public PagedResult<TourDto> GetAllByUserId(int userId)
        {
            var allTours = _tourRepository.GetPaged(1, int.MaxValue);


            var filteredTours = allTours.Results
                               .Where(tour => tour.UserId == userId)
                               .Select(tour => MapToDto(tour))
                               .ToList();

            return new PagedResult<TourDto>(filteredTours, filteredTours.Count());
        }

        public Result<TourDto> GetById(long tourId)
        {
            try
            {
                var tour = _tourRepository.Get(tourId);
                return MapToDto(tour);
            }
            catch (KeyNotFoundException ex)
            {
                return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
            }
        }



        public Result<PagedResult<TourDto>> GetPaged(int page, int pageSize)
        {
            var pagedResult = base.GetPaged(page, pageSize);

            if (pagedResult.IsFailed)
            {
                return Result.Fail(pagedResult.Errors);
            }

            var filteredReviews = pagedResult.Value.Results;


            // Step 2: Return the paged result directly without filtering
            return Result.Ok(pagedResult.Value);

        }

    }
}