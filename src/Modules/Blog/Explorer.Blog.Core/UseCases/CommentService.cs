using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using FluentResults;
using System.Collections.Generic;
using System.Linq;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Blog.Core.UseCases
{
    public class CommentService : ICommentService
    {
        private readonly ICrudRepository<Comment> repository;
        private readonly ICrudRepository<Explorer.Blog.Core.Domain.Blog> blogRepository;
        private readonly IProfileService_Internal _profileService;
        private readonly IMapper mapper;

        public CommentService(ICrudRepository<Comment> repository, ICrudRepository<Explorer.Blog.Core.Domain.Blog> blogRepository,IProfileService_Internal profileService, IMapper mapper)
        {
            this.repository = repository;
            this.blogRepository = blogRepository;
            _profileService = profileService;
            this.mapper = mapper;
        }

        public Result<CommentDTO> Create(long userId, CommentDTO commentDto)
        {
            if (commentDto.BlogId <= 0)
            {
                return Result.Fail<CommentDTO>("Invalid BlogId");
            }
            if (string.IsNullOrWhiteSpace(commentDto.Text))
            {
                return Result.Fail<CommentDTO>("Invalid Text");
            }

            var blog = blogRepository.Get(commentDto.BlogId);
            if (blog == null)
            {
                return Result.Fail<CommentDTO>("Blog not found");
            }

            var comment = mapper.Map<Comment>(commentDto);
            comment.UserId = userId;

            blog.AddComment(comment);
            blogRepository.Update(blog);

            return Result.Ok(mapper.Map<CommentDTO>(comment));
        }

        public Result<List<UserDto>> GetManyUsers(List<long> ids)
        {
            List<UserDto> users = new List<UserDto>();
            List<string> errors = new List<string>();

            var accountImageResult = _profileService.GetManyAccountImage(ids);

            if (accountImageResult.IsFailed)
            {
                errors.AddRange(accountImageResult.Errors.Select(e => e.Message));
                return Result.Fail<List<UserDto>>(errors);
            }

            var accountImageDtos = accountImageResult.Value;

            foreach (var accountImageDto in accountImageDtos)
            {
                var userDto = new UserDto
                {
                    Id = accountImageDto.Id,
                    Name = accountImageDto.Name,
                    LastName = accountImageDto.LastName,
                    Username = accountImageDto.Username,
                    ProfileImage = accountImageDto.ProfileImage
                };

                users.Add(userDto);
            }

            return Result.Ok(users);
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
            if (comment.BlogId != blogId)
            {
                return Result.Fail<CommentDTO>("You can't modify a comment on a different blog.");
            }

            var blog = blogRepository.Get(blogId);
            if (blog == null)
            {
                return Result.Fail<CommentDTO>("Blog not found");
            }

            comment.Text = commentDto.Text;
            comment.UpdateLastModifiedAt();
            repository.Update(comment);

            // Pozivamo metodu Blog entiteta za ažuriranje komentara
            blog.UpdateComment(comment);
            blogRepository.Update(blog); // Ažuriramo blog u skladištu

            return Result.Ok(mapper.Map<CommentDTO>(comment));
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
                return Result.Fail("You can't delete a comment on a different blog.");
            }

            var blog = blogRepository.Get(blogId);
            if (blog == null)
            {
                return Result.Fail("Blog not found");
            }

            repository.Delete(commentId);

            // Pozivamo metodu Blog entiteta za brisanje komentara
            blog.DeleteComment(comment);
            blogRepository.Update(blog); // Ažuriramo blog u skladištu

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
