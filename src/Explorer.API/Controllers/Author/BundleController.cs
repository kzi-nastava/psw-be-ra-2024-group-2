using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/bundles")]
public sealed class BundleController : BaseApiController
{
    private readonly IBundleService _bundleService;

    public BundleController(IBundleService bundleService)
    {
        _bundleService = bundleService;
    }

    [HttpPost]
    public ActionResult<BundleDto> CreateBundle([FromBody] BundleDto bundle)
    {
        var newBundle = _bundleService.CreateBundle(User.UserId(), bundle);
        return CreateResponse(newBundle);
    }

    [HttpPost("add/tour")]
    public ActionResult<BundleDto> AddTourToBundle(int bundleId, [FromBody] BundleItemDto tour)
    {
        var result = _bundleService.AddTourToBundle(User.UserId(), bundleId, tour);
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult<BundleDto> DeleteBundle(int id)
    {
        var result = _bundleService.DeleteBundle(User.UserId(), id);
        return CreateResponse(result);
    }

    [HttpPost("publish")]
    public ActionResult<BundleDto> PublishBundle(int id)
    {
        var result = _bundleService.PublishBundle(User.UserId(), id);
        return CreateResponse(result);
    }

    [HttpGet("api/tourist/bundles/all")]
    public ActionResult<PagedResult<BundleDto>> GetAll()
    {
        var result = _bundleService.GetAll();
        return CreateResponse(result);
    }
}
