using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers
{
    //[Authorize(Policy = "authorPolicy")]
    [Route("api/blog/{blogId:long}")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /*[HttpGet("{id:long}")]
        public ActionResult<CommentDTO> GetById(long id)
        {
            var result = _commentService.GetById(id);
            return CreateResponse(result);
        }*/

        [HttpGet]
        public ActionResult<IEnumerable<CommentWithAuthorDto>> GetByBlogId(long blogId)
        {
            var result = _commentService.GetByBlogId(blogId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            var authorIds = result.Value.Select(c => c.UserId).Distinct().ToList();

            var accountWithImage = _commentService.GetManyUsers(authorIds);

            if (accountWithImage.IsFailed)
            {
                return BadRequest(accountWithImage.Errors);
            }

            var commentWithAuthors = result.Value.Select(comment =>
            {
                var author = accountWithImage.Value.FirstOrDefault(u => u.Id == comment.UserId);
                return new CommentWithAuthorDto
                {
                    Comment = comment,
                    Author = author
                };
            }).ToList();

            return Ok(commentWithAuthors);
        }


        /* [HttpGet]
         public ActionResult<PagedResult<CommentDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
         {
             var result = _commentService.GetPaged(page, pageSize);
             return CreateResponse(result);
         }
        */
        [HttpPost]
        public ActionResult<CommentDTO> Create(long blogId, [FromBody] CommentDTO comment)
        {
            var userId = User.PersonId();
            comment.BlogId = blogId;
            var result = _commentService.Create(userId, comment);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message); // Vraća 400 sa porukom greške
            }

            return CreateResponse(result);
        }

        [HttpPut("{id:long}")]
        public ActionResult<CommentDTO> Update(long id, long blogId, [FromBody] CommentDTO comment)
        {
            var userId = User.PersonId();
            var result = _commentService.Update(id, blogId, userId, comment);
            return CreateResponse(result);
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id, long blogId)
        {
            var userId = User.PersonId();
            var result = _commentService.Delete(id, blogId, userId);

            if (result.IsFailed)
            {
                return NotFound(result.Errors.First().Message);
            }

            return Ok(); // Vraća 200 OK ako je brisanje uspešno
        }

    }
}
