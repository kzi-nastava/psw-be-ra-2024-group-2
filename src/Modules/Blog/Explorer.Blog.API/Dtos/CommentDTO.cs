using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class CommentDTO
    {
        public long BlogId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
