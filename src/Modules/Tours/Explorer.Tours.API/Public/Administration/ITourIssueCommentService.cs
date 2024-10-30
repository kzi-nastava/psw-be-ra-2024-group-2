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
        PagedResult<TourIssueCommentDto> GetPaged(long reportId, int page, int pageSize);
        Result<TourIssueCommentDto> Get(int commentId);
        Result<TourIssueCommentDto> CreateComment(TourIssueCommentDto tourIssueCommentDto);
    }
}
