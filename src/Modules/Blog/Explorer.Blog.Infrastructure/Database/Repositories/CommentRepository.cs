using Explorer.Blog.Core.Domain;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class CommentRepository : CrudDatabaseRepository<Comment, BlogContext>, ICommentRepository
    {
        public CommentRepository(BlogContext dbContext) : base(dbContext)
        {
        }

        public Comment? Get(long id)
        {
            return DbContext.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Update(Comment comment)
        {
            try
            {
                DbContext.Update(comment);
                DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new KeyNotFoundException(e.Message);
            }
            return comment;
        }
    }
}
