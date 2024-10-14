using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    //[Authorize(Policy = "authorPolicy")]
    [Route("api/blog/comment")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id:long}")]
        public ActionResult<CommentDTO> GetById(long id)
        {
            var result = _commentService.GetById(id);
            return CreateResponse(result);
        }

        [HttpGet("blog/{blogId:long}")]
        public ActionResult<IEnumerable<CommentDTO>> GetByBlogId(long blogId)
        {
            var result = _commentService.GetByBlogId(blogId);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<CommentDTO> Create([FromBody] CommentDTO comment)
        {
            var result = _commentService.Create(comment);
            return CreateResponse(result);
        }

        [HttpPut("{id:long}")]
        public ActionResult<CommentDTO> Update([FromBody] CommentDTO comment)
        {
            var result = _commentService.Update(comment);
            return CreateResponse(result);
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        {
            var result = _commentService.Delete(id);
            return CreateResponse(result);
        }
    }
}
