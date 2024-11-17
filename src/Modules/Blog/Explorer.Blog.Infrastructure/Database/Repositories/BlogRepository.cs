using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class BlogRepository : CrudDatabaseRepository<Core.Domain.Blog, BlogContext>, IBlogRepository
    {
        private readonly BlogContext _dbContext;
        public BlogRepository(BlogContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Explorer.Blog.Core.Domain.Blog GetById(int blogId)
        {
            return _dbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
        }

            public Core.Domain.Blog Create(Core.Domain.Blog blog)
        {
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return blog;

        }

        public Explorer.Blog.Core.Domain.Blog UpdateRating(int blogId, string username, RatingType type)
        {
            try
            {
                var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
                blog.RemoveRating(username);
                var rating = blog.AddRating(type, username);
                _dbContext.Blogs.Update(blog);

                _dbContext.SaveChanges();
                return blog;
            }
            catch (DbUpdateException e)
            {
                throw new KeyNotFoundException(e.Message);
            }
        }
        public List<Rating> GetRatingsByBlogId(int blogId)
        {
            var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
            return blog?.Ratings ?? new List<Rating>();
        }
    }
}
