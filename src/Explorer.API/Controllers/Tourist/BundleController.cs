﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;


[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/bundles")]
public class BundleController : BaseApiController
{
    private IBundleService _bundleService;

    public BundleController(IBundleService bundleService)
    {
        _bundleService = bundleService;
    }

    [HttpGet("all")]
    public ActionResult<PagedResult<BundleDto>> GetAll()
    {
        var result = _bundleService.GetAll();
        return CreateResponse(result);
    }
}
