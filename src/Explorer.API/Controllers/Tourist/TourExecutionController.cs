using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tour/execution")]
public class TourExecutionController : BaseApiController
{
    private readonly ITourExecutionService _tourExecutionService;
    private readonly ITourReviewService _tourReviewService;
    private readonly ITourExecutionCheckpointService _tourExecutionCheckpointService;
    private readonly ITourService _tourService;

    public TourExecutionController(ITourReviewService tourReviewService, ITourService tourService, ITourExecutionService tourExecutionService, ITourExecutionCheckpointService tourExecutionCheckpointService)
    {
        _tourExecutionService = tourExecutionService;
        _tourReviewService = tourReviewService;
        _tourService = tourService;
        _tourExecutionCheckpointService = tourExecutionCheckpointService;
    }

    [HttpPost]
    public ActionResult<TourExecutionDto> StartTour([FromBody] int tourId)
    {
        var result = _tourExecutionService.Create(tourId, User.UserId());
       
        return CreateResponse(result);
    }

   


}

