using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Internal;
using FluentResults;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService : CrudService<BlogDto, Explorer.Blog.Core.Domain.Blog>, IBlogService
    {
        public IBlogRepository _blogRepository;
        public ICrudRepository<Explorer.Blog.Core.Domain.Blog> _blogCrudRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProfileService_Internal _profileService;
        private readonly IMapper _mapper;

        public BlogService(ICrudRepository<Explorer.Blog.Core.Domain.Blog> crudRepository, ITransactionRepository transactionRepository, IImageRepository imageRepository, IBlogRepository blogRepository,  IProfileService_Internal profileService, IMapper mapper) : base(crudRepository, mapper)
        {
            _imageRepository = imageRepository;
            _blogCrudRepository = crudRepository;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _profileService = profileService;
            _transactionRepository = transactionRepository;
        }

        public Result<UserDto> GetUser(long id)
        {
            var accountImageResult = _profileService.GetAccountImage(id);

            if (accountImageResult.IsFailed)
                return Result.Fail<UserDto>(accountImageResult.Errors);

            var accountImageDto = accountImageResult.Value;

            var userDto = new UserDto
            {
                Id = accountImageDto.Id,
                Name = accountImageDto.Name,
                LastName = accountImageDto.LastName,
                Username = accountImageDto.Username,
                ProfileImage = accountImageDto.ProfileImage
            };

            return Result.Ok(userDto);
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
        public Result<BlogDto> Create(BlogDto blogDto, int userId)
        {
            try
            {
                var blog = _mapper.Map<Core.Domain.Blog>(blogDto);
                blog.AuthorId = userId;

                var createdBlog = _blogCrudRepository.Create(blog);

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

        public Result<BlogDto> UpdateRating(int blogId, string username, RatingType ratingType)
        {
            var result = _blogRepository.UpdateRating(blogId, username, ratingType);

            return MapToDto(result);
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