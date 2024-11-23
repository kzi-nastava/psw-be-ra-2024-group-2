using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/faq")]
    public class FAQCreateController : BaseApiController
    {
        private readonly IFAQService _fAQService;

        public FAQCreateController(IFAQService fAQService)
        {
            _fAQService = fAQService;
        }

        [HttpPost("{userId}")]
        public ActionResult<FAQDto> Create(long userId, [FromBody] FAQDto faq)
        {
            var results = _fAQService.Create(userId, faq);
            return CreateResponse(results);
        }

        [HttpPut("editFAQ/{faqId}/{userId}")]
        public ActionResult<FAQDto> Update(int faqId, long userId, [FromBody] FAQDto faq)
        {
            var results = _fAQService.Update(faqId, userId, faq);
            return CreateResponse(results);
        }
    }
}
