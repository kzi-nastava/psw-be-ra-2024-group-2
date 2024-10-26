using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
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

        public BlogService(ICrudRepository<Explorer.Blog.Core.Domain.Blog> crudRepository, ITransactionRepository transactionRepository, IMapper mapper) : base(crudRepository, mapper)
        {
            _blogRepository = crudRepository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public Result<BlogDto> Create(BlogDto blogDto)
        {
            try
            {
                var blog = _mapper.Map<Core.Domain.Blog>(blogDto);
                var result = _blogRepository.Create(blog);
                return Result.Ok(_mapper.Map<BlogDto>(result));
            }
            catch (Exception ex)
            {
                return Result.Fail<BlogDto>("An error occurred while creating the blog: " + ex.Message);
            }
        }
    }
}