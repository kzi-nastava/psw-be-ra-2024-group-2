using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/wallet")]

public class TouristWalletController : BaseApiController
{

    private readonly IWalletService_Internal _walletService;

    public TouristWalletController(IWalletService_Internal walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public ActionResult GetWalletBalance(long touristId)
    {
        var result = _walletService.GetWallet(touristId);
        return CreateResponse(result);
    }

}
