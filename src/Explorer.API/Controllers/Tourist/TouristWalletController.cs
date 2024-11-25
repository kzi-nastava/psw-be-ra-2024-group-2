﻿using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/wallet")]

public class TouristWalletController : BaseApiController
{

    private readonly IWalletService _walletService;

    public TouristWalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public ActionResult GetWalletBalance()
    {
        var result = _walletService.GetWallet(User.PersonId());
        return CreateResponse(result);
    }

}
