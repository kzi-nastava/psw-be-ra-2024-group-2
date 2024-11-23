using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Public
{
    public interface IBlogService
    {
        Result<BlogDto> Create(BlogDto blog, int userId);
        Result<PagedResult<BlogDto>> GetPaged(int page, int pageSize);
        Result<BlogDto> Get(int id);
        Result<BlogDto> GetBlogWithRatings(int blogId);
        Result<BlogDto> UpdateRating(int blogId, string username, RatingType ratingType);
        Result<UserDto> GetUser(long id);
        Result<List<UserDto>> GetManyUsers(List<long> userIds);
    }
}
