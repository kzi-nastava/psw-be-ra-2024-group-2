using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Author
{
    public interface ITourSaleService
    {
        public Result<TourSaleDto> Create(TourSaleDto tourSaleDto, int userId);
        public Result<TourSaleDto> Update(int id, TourSaleDto tourSaleDto);
        public Result<bool> Delete(long id);
        public Result<List<TourSaleDto>> GetAll();
        Result<PagedResult<TourSaleDto>> GetPaged(int page, int pageSize);
    }
}
