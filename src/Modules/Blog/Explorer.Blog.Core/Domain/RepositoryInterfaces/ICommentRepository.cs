using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Blog.Core.Domain;
public interface ICommentRepository
{
    public Comment? Get(long id);
    public Comment Update(Comment comment);
}