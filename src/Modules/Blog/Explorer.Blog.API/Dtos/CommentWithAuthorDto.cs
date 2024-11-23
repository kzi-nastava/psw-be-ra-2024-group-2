using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class CommentWithAuthorDto
    {
        public CommentDTO Comment { get; set; }
        public UserDto Author { get; set; }
    }

}
