﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Public.Tourist.DTOs;
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
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;
        public TourExecutionService(ITourExecutionRepository tourExecutionRepository, IMapper mapper, ICrudRepository<Tour> tourRepository, IShoppingCartRepository shoppingCartRepository, ICrudRepository<TourPurchaseToken> tourPurchaseTokenRepository) : base(mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
        }

        public Result<TourExecutionDto> GetByUserId(int userId)
        {
            var result = _tourExecutionRepository.GetByUserId(userId);
            if (result == null)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Tour execution not found");
            }
            return MapToDto(result);
        }


        public Result<TourExecutionDto> Create(int tourId, int userId)
        {
            var tokens = _tourPurchaseTokenRepository.GetPaged(1, int.MaxValue)
                .Results
                .Where(t => t.UserId == userId && t.TourId == tourId)
                .ToList();
            if (tokens.Count == 0)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Tour is not bought");
            }

            var t = _tourRepository.Get(tourId);
            if (t.Status == TourStatus.Archived || t.Status == TourStatus.Published)
            {

                try
                {
                    TourExecution tourExecution = new TourExecution(userId, tourId, TourExecutionStatus.InProgress, DateTime.UtcNow);

                    Tour tour = _tourRepository.Get(tourId);
                    foreach (Checkpoint checkpoint in tour.Checkpoints)
                    {

                        TourExecutionCheckpoint tourExecutionCheckpoint = new TourExecutionCheckpoint(checkpoint.Id, null);
                        tourExecution.TourExecutionCheckpoints.Add(tourExecutionCheckpoint);

                    }
                    var result = _tourExecutionRepository.Create(tourExecution);

                    return MapToDto(result);

                }
                catch (Exception)
                {
                    return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
                }
            }
            return Result.Fail(FailureCode.Conflict).WithError("Tour is in Draft mode");
        }

        public Result<TourExecutionDto> Update(TourExecutionDto tourExecutionDto)
        {
            try
            {
                TourExecution tourExecution = _tourExecutionRepository.Get(tourExecutionDto.Id);

                tourExecution.UpdateTourActivity(DateTime.UtcNow);

                List<TourExecutionCheckpoint> checkpoints = tourExecutionDto.tourExecutionCheckpoints.Select(x => new TourExecutionCheckpoint(x.CheckpointId, x.ArrivalAt)).ToList();
                tourExecution.UpdateCheckpoints(checkpoints);
                var result = _tourExecutionRepository.Update(tourExecution);

                return MapToDto(result);

            }
            catch (Exception)
            {
                return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
            }
        }

        public Result<TourExecutionDto> EndTour(int v, TourExecutionDto dto)
        {
            try
            {
                TourExecution tourExecution = _tourExecutionRepository.Get(dto.Id);
                List<TourExecutionCheckpoint> checkpoints = tourExecution.TourExecutionCheckpoints.Select(x => new TourExecutionCheckpoint(x.CheckpointId, x.ArrivalAt)).ToList();
                bool tourStatus = tourExecution.CheckCheckpoints(checkpoints);
                tourExecution.ChangeEndStatusAndEndingTime(tourStatus);
                var result =  _tourExecutionRepository.Update(tourExecution);
                
                return MapToDto(result);


            }
            catch (Exception)
            {
                return Result.Fail(FailureCode.Conflict).WithError("An unexpected error occurred");
            }

        }
    }
}
