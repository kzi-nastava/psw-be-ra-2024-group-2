using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
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

        public Image AddImage(Image image)
        {
            Images.Add(image);
            return image;
        }

        public Image UpdateImage(Image image)
        {
            var oldImage = Images.Find(x => x.Id == image.Id);
            Images.Remove(oldImage);
            Images.Add(image);
            return image;
        }
        public Image DeleteImage(Image image)
        {
            var deleteImage = Images.Find(x => x.Id == image.Id);
            Images.Remove(deleteImage);
            return image;
        }
        public Comment AddComment(Comment comment)
        {
            Comments.Add(comment);
            return comment;
        }
        public Comment UpdateComment(Comment comment)
        {
            var oldComment = Comments.Find(x => x.Id == comment.Id);
            Comments.Remove(oldComment);
            Comments.Add(comment);
            return comment;
        }

        public Comment DeleteComment(Comment comment)
        {
            var deleteComment = Comments.Find(x => x.Id == comment.Id);
            Comments.Remove(deleteComment);
            return comment;
        }

        public Rating AddRating( RatingType ratingType, string username)
        {
            var rating = new Rating(ratingType, DateTime.UtcNow, username);
            var ratingCheck = Ratings.FirstOrDefault(rating => rating.Username == username);

            if (ratingCheck == null) Ratings.Add(rating);
            return rating;
        }

        public void RemoveRating(string username)
        {
            var ratingToRemove = Ratings.FirstOrDefault(rating => rating.Username == username);
            if (ratingToRemove != null) Ratings.Remove(ratingToRemove);
        }
    }

    public enum Status
    {
        Draft,
        Published,
        Closed
    }
}
