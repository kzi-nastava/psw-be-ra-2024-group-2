using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = Explorer.BuildingBlocks.Core.Domain.Image;

namespace Explorer.Blog.Core.Domain
{
    public class Blog : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }

        public int AuthorId { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating>? Ratings { get; set; } = new List<Rating>();

        public Blog(string title, string description, Status status, int authorId)
        {
            Title = title;
            Description = description;
            Status = status;
            Date = DateTime.UtcNow;
            AuthorId = authorId;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title)) throw new ArgumentException("Invalid Title");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (AuthorId == 0) throw new ArgumentException("Blog must have an Author");
        }
    }
    public enum Status
    {
        Draft,
        Published,
        Closed
    }


}
