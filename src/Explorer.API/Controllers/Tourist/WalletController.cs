using Explorer.Payment.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "allLoggedPolicy")]
[Route("api/wallet")]

public class WalletController : BaseApiController
{

    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public ActionResult GetWalletBalance(long touristId)
    {
        var result = _walletService.GetWallet(touristId);
        return CreateResponse(result);
    }

    [HttpPost("add-ac")]
    //[Authorize(Roles = "Administrator")]
    public ActionResult AddAdventureCoins(long touristId, [FromBody] long amount)
    {
        var result = _walletService.AddFunds(touristId, amount);
        return CreateResponse(result);
    }

}
