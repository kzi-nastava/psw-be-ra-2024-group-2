using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administrator/account")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public ActionResult<PagedResult<AccountDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _accountService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("block")]
        public ActionResult<AccountDto> Block([FromBody] AccountDto account)
        {
            var updatedProfile = _accountService.Block(account.UserId);
            return CreateResponse(updatedProfile);
        }
        [HttpPut("unblock")]
        public ActionResult<AccountDto> Unblock([FromBody] AccountDto account)
        {
            var updatedProfile = _accountService.Unblock(account.UserId);
            return CreateResponse(updatedProfile);
        }
    }
}
