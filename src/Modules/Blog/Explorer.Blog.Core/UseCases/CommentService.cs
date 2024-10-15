using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Blog.Core.UseCases
{
    public class CommentService : ICommentService
    {
        private readonly ICrudRepository<Comment> repository;
        private readonly IMapper mapper;

        public CommentService(ICrudRepository<Comment> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Result<CommentDTO> Create(long UserId, CommentDTO commentDto)
        {
            var comment = mapper.Map<Comment>(commentDto);
            comment.UserId = UserId;
            var result = repository.Create(comment);
            return Result.Ok(mapper.Map<CommentDTO>(result));
        }

        public Result<CommentDTO> Update(long id, long userId, CommentDTO commentDto)
        {
            var comment = repository.Get(id);
            if (comment.UserId != userId)
            {
                return Result.Fail<CommentDTO>("User is not authorized to update this comment.");
            }
            comment.UpdateLastModifiedAt();
            comment.Text = commentDto.Text;
            var result = repository.Update(comment);
            return Result.Ok(mapper.Map<CommentDTO>(result));
        }


        public Result Delete(long commentId, long userId)
        {
            var comment = repository.Get(commentId);
            if (comment.UserId != userId)
            {
                return Result.Fail("User is not authorized to delete this comment.");
            }
            repository.Delete(commentId);
            return Result.Ok();
        }

        public Result<CommentDTO> GetById(long commentId)
        {
            var result = repository.Get(commentId);
            return result != null ? Result.Ok(mapper.Map<CommentDTO>(result)) : Result.Fail("Comment not found");
        }

        public Result<IEnumerable<CommentDTO>> GetByBlogId(long blogId)
        {
            var comments = repository.GetPaged(1, int.MaxValue).Results;
            var filteredComments = comments.Where(c => c.BlogId == blogId);
            return Result.Ok(mapper.Map<IEnumerable<CommentDTO>>(filteredComments));
        }
    }
}
