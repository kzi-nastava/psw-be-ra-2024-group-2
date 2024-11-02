using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly ICrudRepository<Equipment> _equipmentRepository;
        private readonly ICrudRepository<Checkpoint> _checkpointRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TourService(ICrudRepository<Tour> tourRepository, ICrudRepository<Equipment> equipmentRepository, ICrudRepository<Checkpoint> checkpointRepository, IMapper mapper, ITransactionRepository transactionRepository) : base(tourRepository, mapper)
        {
            _tourRepository = tourRepository;
            _equipmentRepository = equipmentRepository;
            _checkpointRepository = checkpointRepository;
            _transactionRepository = transactionRepository;
        }

        public Result<TourDto> UpdateTour(TourDto tourDto, long userId)
        {
            try
            {
                if (tourDto.UserId != userId)
                    return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add equipment to this tour");

                _transactionRepository.BeginTransaction();
                Tour tour = _tourRepository.Get(tourDto.Id);

                tour.Equipment.Clear();

                foreach (var elementId in tourDto.Equipment)
                {
                    var newEquipment = _equipmentRepository.Get(elementId);
                    tour.Equipment.Add(newEquipment);
                }

                if(tourDto.Status.ToString() == "Published") 
                {
                    tour.UpdatePublishDate(DateTime.UtcNow);
                    tour.UpdateArhivedDate(null);
                }
                else if(tourDto.Status.ToString() == "Archived") 
                {
                    tour.UpdatePublishDate(null);
                    tour.UpdateArhivedDate(DateTime.UtcNow);
                }

                tour.UpdateStatus(tourDto.Status);
                tour.UpdatePrice(tourDto.Price);

                _tourRepository.Update(tour);
                _transactionRepository.CommitTransaction();
                return MapToDto(tour);
            }
            catch (Exception e)
            {
                if (_transactionRepository.HasActiveTransacation())
                    _transactionRepository.RollbackTransaction();

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
            try
            {
                dto.UserId = userId;

                _transactionRepository.BeginTransaction();

                Tour tour = new(dto.UserId,
                    dto.Name,
                    dto.Description,
                    dto.Difficulty,
                    dto.Tag,
                    dto.Status,
                    dto.Price);

                Validate(dto);

                tour.TourDurationByTransports = dto.TourDurationByTransportDtos
                    .Select(tourDurationByTransportDto => 
                        new TourDurationByTransport(
                            Enum.Parse<TransportType>(tourDurationByTransportDto.Transport), 
                            tourDurationByTransportDto.Duration))
                    .ToList();

                var result = _tourRepository.Create(tour);

                _transactionRepository.CommitTransaction();

                return MapToDto(result);

            } catch(Exception)
            {
                _transactionRepository.RollbackTransaction();

                return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
            }
        }

        // Price should not be 0
        // Status should be draft at start
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

        public Result DeleteById(int tourId)
        {
            return base.Delete(tourId);
        }
    }
}