using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/tour")]
public class TourSearchController : BaseApiController
{
    private readonly ITourService _tourService;

    public TourSearchController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpPost("nearby")]
    public ActionResult<PagedResult<TourDto>> GetToursNearby([FromBody] LocationDto locationDto)
    {
        var result = _tourService.GetToursNearby(User.UserId(), locationDto);
        return CreateResponse(result.ToResult());
    }
}
