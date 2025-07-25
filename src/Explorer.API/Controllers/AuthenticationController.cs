using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Route("api/users")]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public ActionResult<AuthenticationTokensDto> RegisterTourist([FromBody] AccountRegistrationDto account)
    {
        var result = _authenticationService.RegisterTourist(account);
        return CreateResponse(result);
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationTokensDto> Login([FromBody] CredentialsDto credentials)
    {
        var result = _authenticationService.Login(credentials);
        return CreateResponse(result);
    }

    [HttpGet("getUsersWithInterests")]
    public ActionResult<List<string>> GetUsersWithInterests([FromQuery] string interests)
    {
        var result = _authenticationService.GetUsersWithMatchingInterests(interests);

        if (!result.IsFailed)
        {
            return Ok(result.Value); // Return the list of emails if successful
        }
        else
        {
            return NotFound(new { Message = string.Join(", ", result.MapErrors) }); // Return an error if no users are found
        }
    }



    [HttpGet("GetWallet/{userId}")]
    public ActionResult<WalletDto> GetWallet(long userId)
    {
        var result = _authenticationService.GetUserWallet(userId);
        //if (result.IsFailure) return NotFound(result.Error);

        return CreateResponse(result);
    }

}