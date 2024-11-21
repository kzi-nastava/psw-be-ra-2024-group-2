using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/user/blog")]
    public class BlogController : BaseApiController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public ActionResult<PagedResult<BlogWithAuthorDto>> GetAll()
        {
            var result = _blogService.GetPaged(0, 0);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            var blogs = result.Value;

            var authorIds = blogs.Results.Select(b => b.AuthorId).Distinct().ToList();
            var usersResult = _blogService.GetManyUsers(authorIds);

            if (usersResult.IsFailed)
            {
                return BadRequest(usersResult.Errors);
            }

            var users = usersResult.Value;

            var blogWithAuthorDtos = blogs.Results.Select(blog =>
            {
                var user = users.FirstOrDefault(u => u.Id == blog.AuthorId);
                return new BlogWithAuthorDto
                {
                    Blog = blog,
                    Author = user
                };
            }).ToList();

            var pagedResult = new PagedResult<BlogWithAuthorDto>(blogWithAuthorDtos, blogs.TotalCount);

            return Ok(pagedResult);
        }


        [HttpGet("{id}")]
        public ActionResult<BlogWithAuthorDto> GetById(int id)
        {
            var result = _blogService.Get(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            var blog = result.Value;

            var userResult = _blogService.GetUser(blog.AuthorId);

            if (userResult.IsFailed)
            {
                return BadRequest(userResult.Errors); 
            }

            var user = userResult.Value;

            var blogWithAuthorDto = new BlogWithAuthorDto
            {
                Blog = blog,
                Author = user
            };

            return Ok(blogWithAuthorDto);
        }

        [HttpPost]
        public ActionResult<BlogDto> Create([FromBody] BlogDto dto)
        {
            int userId = User.PersonId();
            var newBlog = _blogService.Create(dto, userId);
            return CreateResponse(newBlog);
        }

        [HttpPut("rating/{blogId:int}/{username}/{ratingType}")]
        public ActionResult<BlogDto> UpdateRating(int blogId, string username, string ratingType)
        {
            if (!Enum.TryParse<RatingType>(ratingType, true, out var parsedRatingType))
            {
                return BadRequest($"Invalid rating type: {ratingType}");
            }

            var result = _blogService.UpdateRating(blogId, username, parsedRatingType);
            return CreateResponse(result);
        }

        [HttpGet("blog/{id}/with-ratings")]
        public ActionResult<BlogDto> GetBlogWithRatings(int id)
        {
            var result = _blogService.GetBlogWithRatings(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
