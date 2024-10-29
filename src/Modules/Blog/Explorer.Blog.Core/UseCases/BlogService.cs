using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.Domain;
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
        public ICrudRepository<Explorer.Blog.Core.Domain.Blog> _blogRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public BlogService(ICrudRepository<Explorer.Blog.Core.Domain.Blog> crudRepository, ITransactionRepository transactionRepository, IImageRepository imageRepository, IMapper mapper) : base(crudRepository, mapper)
        {
            _imageRepository = imageRepository;
            _blogRepository = crudRepository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public Result<BlogDto> Create(BlogDto blogDto)
        {
            try
            {
                var blog = _mapper.Map<Core.Domain.Blog>(blogDto);

                var createdBlog = _blogRepository.Create(blog);

                if (blogDto.Images != null && blogDto.Images.Any())
                {
                    foreach (var image in blogDto.Images)
                    {
                        _imageRepository.Create(image);
                    }
                }

                return Result.Ok(_mapper.Map<BlogDto>(createdBlog));
            }
            catch (Exception ex)
            {
                return Result.Fail<BlogDto>("An error occurred while creating the blog: " + ex.Message);
            }
        }
    }
}