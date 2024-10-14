using Explorer.Blog.API.Dtos;
using FluentResults;
using System.Collections.Generic;

namespace Explorer.Blog.API.Public
{
    public interface ICommentService
    {
        Result<CommentDTO> Create(CommentDTO comment);
        Result<CommentDTO> Update(CommentDTO comment);
        Result Delete(long commentId);
        Result<CommentDTO> GetById(long commentId);
        Result<IEnumerable<CommentDTO>> GetByBlogId(long blogId);
    }
}
