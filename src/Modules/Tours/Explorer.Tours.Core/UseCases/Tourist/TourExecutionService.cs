using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TourExecutionService : CrudService<TourExecutionDto,TourExecution>, ITourExecutionService
    {
        private readonly ICrudRepository<TourExecution> _tourExecutionRepository;
        private readonly ICrudRepository<TourExecutionCheckpoint> _tourExecutionCheckpointRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        public TourExecutionService(ICrudRepository<TourExecution> tourExecutionRepository, IMapper mapper,ICrudRepository<Tour> tourRepository, ICrudRepository<TourExecutionCheckpoint> tourExecutionCheckpointRepository) : base(tourExecutionRepository, mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
            _tourExecutionCheckpointRepository = tourExecutionCheckpointRepository;
        }

        public Result<TourExecutionDto> Create(int tourId, int userId)
        {
            try
            {
                TourExecution tourExecution = new TourExecution(userId, tourId, TourExecutionStatus.InProgress, DateTime.UtcNow, DateTime.UtcNow);
                var result = _tourExecutionRepository.Create(tourExecution);
                Tour tour = _tourRepository.Get(tourId);
                foreach (Checkpoint checkpoint in tour.Checkpoints)
                {

                    TourExecutionCheckpoint tourExecutionCheckpoint = new TourExecutionCheckpoint(result.Id, CheckpointStatus.NotCompleted, DateTime.UtcNow, checkpoint.Id);
                    _tourExecutionCheckpointRepository.Create(tourExecutionCheckpoint);

                }



                return MapToDto(result);

            }
            catch(Exception) 
            {
                return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
            }

        }
    }
}
