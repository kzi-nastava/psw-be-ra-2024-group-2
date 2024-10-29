using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourIssueCommentService
    {
        Result<PagedResult<TourIssueCommentDto>> GetPaged(int page, int pageSize);
        Result<TourIssueCommentDto> Get(int commentId);
        Result<TourIssueCommentDto> Create(TourIssueCommentDto tourIssueCommentDto);
    }
}
