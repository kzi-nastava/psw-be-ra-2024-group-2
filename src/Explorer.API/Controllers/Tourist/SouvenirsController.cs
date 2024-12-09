using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;


[Authorize(Policy = "allLoggedPolicy")]
[Route("api/souvenirs")]
public sealed class SouvenirsController : BaseApiController
{
    private readonly ITourSouvenirService _tourSouvenirService;

    public SouvenirsController(ITourSouvenirService tourSouvenirService)
    {
        _tourSouvenirService = tourSouvenirService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourSouvenirDto>> GetAll()
    {
        var result = _tourSouvenirService.GetAll();
        return CreateResponse(result);
    }
}
