using Explorer.Blog.API.Dtos;
using FluentResults;
using System.Collections.Generic;

namespace Explorer.Blog.API.Public
{
    public interface ICommentService
    {
        Result<CommentDTO> Create(long UserId, CommentDTO comment);
        Result<CommentDTO> Update(long id, long UserId, CommentDTO comment);
        Result Delete(long id, long userId);
        Result<CommentDTO> GetById(long commentId);
        Result<IEnumerable<CommentDTO>> GetByBlogId(long blogId);
    }
}
