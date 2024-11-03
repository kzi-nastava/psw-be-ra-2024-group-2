using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Infrastructure.Database;
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

        public Core.Domain.Blog Create(Core.Domain.Blog blog)
        {
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return blog;

        }
    }
}
