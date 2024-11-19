using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogWithAuthorDto
    {
        public BlogDto Blog { get; set; }
        public UserDto Author { get; set; }
    }
}
