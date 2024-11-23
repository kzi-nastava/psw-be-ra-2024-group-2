using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title {  get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public long AuthorId { get; set; }
        public DateTime Date { get; set;  }
        public List<Image?> Images { get; set; } = new List<Image>();
        public List<RatingDto?> Ratings { get; set; } = new List<RatingDto>();
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

    }
    public enum Status
    {
        Draft,
        Published,
        Closed
    }
}
