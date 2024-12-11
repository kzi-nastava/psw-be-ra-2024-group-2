using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class CheckpointService : CrudService<CheckpointDto, Checkpoint>, ICheckpointService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICrudRepository<Checkpoint> _checkpointRepository;
        private readonly ICrudRepository<Tour> _tourRepository;

        public CheckpointService(ICrudRepository<Checkpoint> repository, IMapper mapper, IImageRepository imageRepository, ICrudRepository<Checkpoint> checkpointRepository, ICrudRepository<Tour> tourRepository) : base(repository, mapper)
        {
            _imageRepository = imageRepository;
            _checkpointRepository = checkpointRepository;
            _tourRepository = tourRepository;
        }

        public List<TourExecutionCheckpointDto> CheckDistance(List<TourExecutionCheckpointDto> execCheckpoints, double lon, double lat)
        {


                foreach(TourExecutionCheckpointDto exeCheckpoint in execCheckpoints)
                {
                    if (exeCheckpoint.ArrivalAt != null)
                    continue;

                    Checkpoint cp = _checkpointRepository.Get(exeCheckpoint.CheckpointId);
                
                    if (cp.CheckRadius(lon, lat))
                    {
                    exeCheckpoint.ArrivalAt = DateTime.UtcNow;
                    break;
                    }
                }

            return execCheckpoints;
        }
        public PagedResult<CheckpointDto> GetAllById(List<long> ids)
        {
            var allCheckpoints = _checkpointRepository.GetPaged(1, int.MaxValue);
            var filteredcheckpoints = allCheckpoints.Results
                               .Where(checkpoint => ids.Contains(checkpoint.Id))
                               .Select(checkpoint => MapToDto(checkpoint))
                               .ToList();

            return new PagedResult<CheckpointDto>(filteredcheckpoints, filteredcheckpoints.Count());
        }

        public Result<PagedResult<CheckpointDto>> GetAllByTourId(long tourId)
        {
            var tour = _tourRepository.Get(tourId);
            if (tour == null)
            {
                throw new KeyNotFoundException($"Tour with ID {tourId} not found.");
            }

            var checkpointDtos = tour.Checkpoints.Select(checkpoint => MapToDto(checkpoint)).ToList();
            var filteredCheckpointsResult = new PagedResult<CheckpointDto>(checkpointDtos, checkpointDtos.Count);
            return Result.Ok(filteredCheckpointsResult);
        }


    }
}
