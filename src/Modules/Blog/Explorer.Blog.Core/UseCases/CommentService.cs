﻿using AutoMapper;
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
        private readonly BlogService blogService;
        public CommentService(ICrudRepository<Comment> repository, IMapper mapper, BlogService blogService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.blogService = blogService;
        }

        public Result<CommentDTO> Create(long userId, CommentDTO commentDto)
        {
            // Proveri validnost podataka pre nego što pokušaš da kreiraš komentar
            if (commentDto.BlogId <= 0)
            {
                return Result.Fail<CommentDTO>("Invalid BlogId");
            }
            if (string.IsNullOrWhiteSpace(commentDto.Text))
            {
                return Result.Fail<CommentDTO>("Invalid Text");
            }

            var comment = mapper.Map<Comment>(commentDto);
            comment.UserId = userId;
            var result = repository.Create(comment);
            blogService.AddCommentToBlog(commentDto.BlogId, result);
            return Result.Ok(mapper.Map<CommentDTO>(result));
        }



        public Result<CommentDTO> Update(long id, long blogId, long userId, CommentDTO commentDto)
        {
            var comment = repository.Get(id);
            if (comment == null)
            {
                return Result.Fail<CommentDTO>("Comment not found.");
            }
            if (comment.UserId != userId)
            {
                return Result.Fail<CommentDTO>("User is not authorized to update this comment.");
            }
            if(comment.BlogId != blogId)
            {
                return Result.Fail<CommentDTO>("You can't modify comment on other blog.");
            }
            comment.UpdateLastModifiedAt();
            comment.Text = commentDto.Text;
            var result = repository.Update(comment);
            return Result.Ok(mapper.Map<CommentDTO>(result));
        }



        public Result Delete(long commentId, long blogId, long userId)
        {
            var comment = repository.Get(commentId);
            if (comment == null)
            {
                return Result.Fail("Comment not found."); 
            }
            if (comment.UserId != userId)
            {
                return Result.Fail("User is not authorized to delete this comment.");
            }
            if (comment.BlogId != blogId)
            {
                return Result.Fail("You can't delete comment on other blog.");
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
        public Result<PagedResult<CommentDTO>> GetPaged(int page, int pageSize)
        {
            var pagedComments = repository.GetPaged(page, pageSize);
            var mappedComments = mapper.Map<List<CommentDTO>>(pagedComments.Results);
            var result = new PagedResult<CommentDTO>(mappedComments, pagedComments.TotalCount);

            return Result.Ok(result);
        }

    }
}
