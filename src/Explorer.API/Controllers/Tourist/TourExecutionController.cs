using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tour/execution")]
public class TourExecutionController : BaseApiController
{
    private readonly ITourExecutionService _tourExecutionService;
    private readonly ITourReviewService _tourReviewService;
    private readonly ITourService _tourService;
    private readonly ICheckpointService _checkpointService;

    public TourExecutionController(ITourReviewService tourReviewService, ITourService tourService, ITourExecutionService tourExecutionService, ICheckpointService checkpointService)
    {
        _tourExecutionService = tourExecutionService;
        _tourReviewService = tourReviewService;
        _tourService = tourService;
        _checkpointService = checkpointService;
    }

    [HttpPost("{tourId}")]
    public ActionResult<TourExecutionDto> StartTour(int tourId)
    {
        var result = _tourExecutionService.Create(tourId, User.UserId());

        return CreateResponse(result);
    }
    [HttpPost("checkTouristPosition")]
    public ActionResult<TourExecutionDto> CheckTouristPosition([FromBody] TouristPosition tourPosition)
    {
        var executionResult = _tourExecutionService.GetByUserId(User.UserId());
        var checkpointResult = _checkpointService.CheckDistance(executionResult.Value.tourExecutionCheckpoints, tourPosition.Longitude, tourPosition.Latitude);

        TourExecutionDto executionDto = executionResult.Value;
        executionDto.tourExecutionCheckpoints = checkpointResult;
       
        var result = _tourExecutionService.Update(executionDto);

        return CreateResponse(result);
    }
    [HttpGet("load")]
    public ActionResult<TourExecutionDto> LoadTourExecution()
    {
        var result = _tourExecutionService.GetByUserId(User.UserId());
        return CreateResponse(result);
    }

    [HttpGet("loadTour/{tourId}")]
    public ActionResult<TourDto> GetById(long tourId)
    {
        var tour = _tourService.GetById(tourId);
        return CreateResponse(tour);
    }
    [HttpPost("checkpoints/getSome")]
    public ActionResult<PagedResult<CheckpointDto>> GetAllById([FromBody] List<long> ids)
    {
        var allCheckpoints = _checkpointService.GetAllById(ids);
        return CreateResponse(allCheckpoints.ToResult());
    }

    [HttpPost("end")]
    public ActionResult<TourExecutionDto> EndTour(TourExecutionDto dto)
    {
        var result = _tourExecutionService.EndTour(User.UserId(), dto);
        return CreateResponse(result);
    }

}

