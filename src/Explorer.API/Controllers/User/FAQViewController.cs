using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/faqView")]
    public class FAQViewController : BaseApiController
    {

        private readonly IFAQService _fAQService;

        public FAQViewController(IFAQService fAQService)
        {
            _fAQService = fAQService;
        }

        [HttpGet]
        public ActionResult<PagedResult<FAQDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _fAQService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}
