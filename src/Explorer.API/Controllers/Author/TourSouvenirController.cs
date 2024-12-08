using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/souvenirs")]
public sealed class TourSouvenirController : BaseApiController
{
    private readonly ITourSouvenirService _tourSouvenirService;

    public TourSouvenirController(ITourSouvenirService tourSouvenirService)
    {
        _tourSouvenirService = tourSouvenirService;
    }

    [HttpPost]
    public ActionResult<TourSouvenirDto> CreateSouvenir([FromBody] TourSouvenirDto souvenir)
    {
        var newSouvenir = _tourSouvenirService.CreateSouvenir(User.UserId(), souvenir);
        return CreateResponse(newSouvenir);
    }

    [HttpPut]
    public ActionResult<TourSouvenirDto> UpdateSouvenir([FromBody] TourSouvenirDto souvenir)
    {
        var updatedSouvenir = _tourSouvenirService.UpdateSouvenir(User.UserId(), souvenir);
        return CreateResponse(updatedSouvenir);
    }

    [HttpGet]
    public ActionResult<PagedResult<TourSouvenirDto>> GetMySouvenirs()
    {
        var result = _tourSouvenirService.GetMySouvenirs(User.UserId());
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult<TourSouvenirDto> DeleteSouvenir(long id)
    {
        var result = _tourSouvenirService.DeleteSouvenir(User.UserId(), id);
        return CreateResponse(result);
    }

    [HttpPut("publish")]
    public ActionResult<TourSouvenirDto> PublishSouvenir(long id)
    {
        var result = _tourSouvenirService.PublishSouvenir(User.UserId(), id);
        return CreateResponse(result);
    }

    [HttpPut("update/count")]
    public ActionResult<TourSouvenirDto> UpdateSouvenirCount(long id, int count)
    {
        var result = _tourSouvenirService.UpdateSouvenirCount(User.UserId(), id, count);
        return CreateResponse(result);
    }
}
