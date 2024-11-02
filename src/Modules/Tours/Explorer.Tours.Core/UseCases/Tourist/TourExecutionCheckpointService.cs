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
    public class TourExecutionCheckpointService: CrudService<TourExecutionCheckpointDto,TourExecutionCheckpoint>, ITourExecutionCheckpointService
    {
        private readonly ICrudRepository<TourExecutionCheckpoint> _tourExecutionCheckpointRepository;
        private readonly ICrudRepository<Checkpoint> _checkpointRepository;
        private readonly ICrudRepository<TourExecution> _tourExecutionRepository;

        public TourExecutionCheckpointService(ICrudRepository<TourExecutionCheckpoint> tourExecutionCheckpointRepository, IMapper mapper,ICrudRepository<TourExecution>  tourExecutionRpeository ,ICrudRepository<Checkpoint> checkpointRepository) : base(tourExecutionCheckpointRepository, mapper)
        {
            _tourExecutionCheckpointRepository = tourExecutionCheckpointRepository;
            _checkpointRepository = checkpointRepository;
            _tourExecutionRepository = tourExecutionRpeository;
        }

        public Result Create(int tourExecutionId, TourDto tourDto)
        {
            


            foreach (int id in tourDto.Checkpoints)
            {

                TourExecutionCheckpoint tourExecutionCheckpoint = new TourExecutionCheckpoint(tourExecutionId,CheckpointStatus.NotCompleted,DateTime.UtcNow,id);
                _tourExecutionCheckpointRepository.Create(tourExecutionCheckpoint);

            }


            return Result.Ok();
        }
    }
}
