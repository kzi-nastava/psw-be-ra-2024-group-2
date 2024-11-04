using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService : CrudService<BlogDto, Explorer.Blog.Core.Domain.Blog>, IBlogService
    {
        public IBlogRepository _blogRepository;
        public ICrudRepository<Explorer.Blog.Core.Domain.Blog> _blogCrudRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public BlogService(ICrudRepository<Explorer.Blog.Core.Domain.Blog> crudRepository, ITransactionRepository transactionRepository, IImageRepository imageRepository, IBlogRepository blogRepository, IMapper mapper) : base(crudRepository, mapper)
        {
            _imageRepository = imageRepository;
            _blogCrudRepository = crudRepository;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _transactionRepository = transactionRepository;
        }

        public Result<BlogDto> Create(BlogDto blogDto)
        {
            try
            {
                var blog = _mapper.Map<Core.Domain.Blog>(blogDto);

                var createdBlog = _blogCrudRepository.Create(blog);

                if (blogDto.Images != null && blogDto.Images.Any())
                {
                    foreach (var image in blogDto.Images)
                    {
                        _imageRepository.Create(image);
                        blog.AddImage(image);
                    }
                }
                
                return Result.Ok(_mapper.Map<BlogDto>(createdBlog));
            }
            catch (Exception ex)
            {
                return Result.Fail<BlogDto>("An error occurred while creating the blog: " + ex.Message);
            }
        }

        public Result<BlogDto> UpdateRating(int blogId, string username, RatingType ratingType)
        {
            var result = _blogRepository.UpdateRating(blogId, username, ratingType);

            return MapToDto(result);
        }

        public void AddCommentToBlog(long blogId, Comment comment)
        {
            var blog = _blogRepository.GetById((int)blogId);
            if (blog == null) throw new Exception("Blog not found");

            blog.Comments.Add(comment);
            _blogRepository.AddCommentToBlog(blogId, comment);
        }

        public Result<BlogDto> GetBlogWithRatings(int blogId)
        {
            var blog = _blogRepository.GetById(blogId);
            if (blog == null)
            {
                return Result.Fail<BlogDto>("Blog not found.");
            }

            var blogDto = _mapper.Map<BlogDto>(blog);
            return Result.Ok(blogDto);
        }
    }
}