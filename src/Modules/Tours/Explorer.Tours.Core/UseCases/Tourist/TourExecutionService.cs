using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
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
    public class TourExecutionService : BaseService<TourExecutionDto,TourExecution>, ITourExecutionService
    {
        private readonly ITourExecutionRepository _tourExecutionRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        public TourExecutionService(ITourExecutionRepository tourExecutionRepository, IMapper mapper, ICrudRepository<Tour> tourRepository) : base(mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
        }

        public Result<TourExecutionDto> Create(int tourId, int userId)
        {
            try
            {
                TourExecution tourExecution = new TourExecution(userId, tourId, TourExecutionStatus.InProgress, DateTime.UtcNow);
                
                Tour tour = _tourRepository.Get(tourId);
                foreach (Checkpoint checkpoint in tour.Checkpoints)
                {

                    TourExecutionCheckpoint tourExecutionCheckpoint = new TourExecutionCheckpoint(checkpoint.Id);
                    tourExecution.TourExecutionCheckpoints.Add(tourExecutionCheckpoint);

                }
                var result = _tourExecutionRepository.Create(tourExecution);
                
                return MapToDto(result);

            }
            catch(Exception) 
            {
                return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
            }

        }
    }
}
