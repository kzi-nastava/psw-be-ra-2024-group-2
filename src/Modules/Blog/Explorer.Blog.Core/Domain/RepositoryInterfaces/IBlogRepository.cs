using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces
{
    public interface IBlogRepository
    {
        Blog GetById(int blogId);

        Blog Create(Blog blog);
        Blog UpdateRating(int blogId, string username, RatingType type);
        Blog AddCommentToBlog(long blogId, Comment comment);
    }
}
