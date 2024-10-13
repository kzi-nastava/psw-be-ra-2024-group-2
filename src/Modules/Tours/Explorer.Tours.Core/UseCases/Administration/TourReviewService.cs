using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {
        public TourReviewService(ICrudRepository<TourReview> repository, IMapper mapper) : base(repository, mapper) { }

        public Result<PagedResult<TourReviewDto>> GetPagedByTourId(int tourId, int page, int pageSize)
        {
            // Step 1: Fetch paged data from the base service
            var pagedResult = base.GetPaged(page, pageSize);

            if (pagedResult.IsFailed)
            {
                return Result.Fail(pagedResult.Errors);
            }

            // Step 2: Filter the results by TourId
            var filteredReviews = pagedResult.Value.Results
                .Where(r => r.TourId == tourId)
                .ToList();

            // Step 3: Create a new PagedResult based on filtered data
            var filteredPagedResult = new PagedResult<TourReviewDto>(filteredReviews, pageSize);

            return Result.Ok(filteredPagedResult);
        }
    }
}
