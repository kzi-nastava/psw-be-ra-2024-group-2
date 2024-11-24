using Explorer.Payment.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator;

[Authorize(Roles = "Administrator")]
[Route("api/wallet")]

public class AdministratorWalletController : BaseApiController
{

    private readonly IWalletService _walletService;

    public AdministratorWalletController(IWalletService walletService)
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
    public ActionResult AddAdventureCoins(long touristId, [FromBody] long amount)
    {
        var result = _walletService.AddFunds(touristId, amount);
        return CreateResponse(result);
    }

}
