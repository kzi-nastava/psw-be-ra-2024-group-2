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

    [HttpGet]
    public ActionResult<PagedResult<FullBundleDto>> GetMyBundles()
    {
        var userId = User.UserId();
        var result = _bundleService.GetMyBundles(userId);
        return CreateResponse(result);
    }

    [HttpPost("add/tour")]
    public ActionResult<BundleDto> AddTourToBundle(long bundleId, [FromBody] BundleItemDto tour)
    {
        var result = _bundleService.AddTourToBundle(User.UserId(), bundleId, tour);
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult<BundleDto> DeleteBundle(long id)
    {
        var result = _bundleService.DeleteBundle(User.UserId(), id);
        return CreateResponse(result);
    }

    [HttpPost("publish")]
    public ActionResult<BundleDto> PublishBundle(long id)
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

    // Path param
    [HttpPut]
    public ActionResult<BundleDto> UpdateBundle(long id, [FromBody] BundleDto bundle)
    {
        var result = _bundleService.UpdateBundle(User.UserId(), id, bundle);
        return CreateResponse(result);
    }
}
