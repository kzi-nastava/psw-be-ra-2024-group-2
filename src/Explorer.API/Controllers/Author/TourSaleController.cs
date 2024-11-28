using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Microsoft.AspNetCore.Mvc.Route("api/user/tourSale")]
    public class TourSaleController : BaseApiController
    {
        private readonly ITourSaleService _tourSaleService;

        public TourSaleController(ITourSaleService tourSaleService)
        {
            _tourSaleService = tourSaleService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourSaleDto>> GetAll()
        {
            var result = _tourSaleService.GetAll();
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<TourSaleDto> CreateTourSale([FromBody] TourSaleDto dto)
        {
            var result =_tourSaleService.Create(dto, User.PersonId());

            if (!result.IsSuccess)
            {
                return BadRequest(result); 
            }

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<TourSaleDto> UpdateTourSale(int id, [FromBody] TourSaleDto dto)
        {
            var result = _tourSaleService.Update(id, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpDelete]
        public ActionResult<bool> DeleteTourSale(long id)
        {
            var result = _tourSaleService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}
