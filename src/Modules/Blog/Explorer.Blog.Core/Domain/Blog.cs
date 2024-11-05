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
        public Status Status { get; private set; }

        public int AuthorId { get; set; }
        public List<Image> Images { get; private set; } = new List<Image>();
        public List<Comment>? Comments { get; private set; } = new List<Comment>();
        public List<Rating>? Ratings { get; private set; } = new List<Rating>();

        public Blog(string title, string description, Status status, int authorId)
        {
            Title = title;
            Description = description;
            Status = status;
            Date = DateTime.UtcNow;
            AuthorId = authorId;
            Validate();
            CheckStatus();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title)) throw new ArgumentException("Invalid Title");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (AuthorId == 0) throw new ArgumentException("Blog must have an Author");
        }

        private void CheckStatus()
        {
            /*
            Console.WriteLine("Blog Details:");
            Console.WriteLine($"- Title: {Title}");
            Console.WriteLine($"- Description: {Description}");

            // Ispis rejtinga (Rating) za proveru podataka
            Console.WriteLine("Ratings:");
            if (Ratings != null && Ratings.Any())
            {
                foreach (var rating in Ratings)
                {
                    Console.WriteLine($"  - Username: {rating.Username}, CreatedAt: {rating.CreatedAt}, RatingType: {rating.RatingType}");
                }
            }
            if (Comments != null && Comments.Any())
            {
                foreach (var rating in Comments)
                {
                    Console.WriteLine($"  - Username: {rating.Text}, CreatedAt: {rating.CreatedAt}");
                }
            }
            else
            {
                Console.WriteLine("  No ratings found.");
            }

            */
            int upvotes = Ratings?.Count(rating => rating.RatingType == RatingType.Upvote) ?? 0;
            int downvotes = Ratings?.Count(rating => rating.RatingType == RatingType.Downvote) ?? 0;
            int score = upvotes - downvotes;

            int commentCount = Comments?.Count ?? 0;

            // Ažuriranje statusa na osnovu uslova
            if (score < -10)
            {
                Status = Status.Closed;
            }
            else if (score > 500 && commentCount > 30)
            {
                Status = Status.Famous;
            }
            else if (score > 100 || commentCount > 10)
            {
                Status = Status.Active;
            }
            else
            {
                Status = Status.Published;
            }

            Console.WriteLine($"Updated Status: {Status}");
        }




        public Image AddImage(Image image)
        {
            EnsureNotClosed();
            Images.Add(image);
            return image;
        }

        public Image UpdateImage(Image image)
        {
            EnsureNotClosed();
            var oldImage = Images.Find(x => x.Id == image.Id);
            Images.Remove(oldImage);
            Images.Add(image);
            return image;
        }

        public Image DeleteImage(Image image)
        {
            EnsureNotClosed();
            var deleteImage = Images.Find(x => x.Id == image.Id);
            Images.Remove(deleteImage);
            return image;
        }

        public Comment AddComment(Comment comment)
        {
            EnsureNotClosed();
            Comments.Add(comment);
            CheckStatus();
            return comment;
        }

        public Comment UpdateComment(Comment comment)
        {
            EnsureNotClosed();
            var oldComment = Comments.Find(x => x.Id == comment.Id);
            Comments.Remove(oldComment);
            Comments.Add(comment);
            CheckStatus();
            return comment;
        }

        public Comment DeleteComment(Comment comment)
        {
            EnsureNotClosed();
            var deleteComment = Comments.Find(x => x.Id == comment.Id);
            Comments.Remove(deleteComment);
            CheckStatus();
            return comment;
        }

        public Rating AddRating(RatingType ratingType, string username)
        {
            EnsureNotClosed();
            var rating = new Rating(ratingType, DateTime.UtcNow, username);
            var ratingCheck = Ratings.FirstOrDefault(rating => rating.Username == username);

            if (ratingCheck == null) Ratings.Add(rating);
            CheckStatus();
            return rating;
        }

        public void RemoveRating(string username)
        {
            EnsureNotClosed();
            var ratingToRemove = Ratings.FirstOrDefault(rating => rating.Username == username);
            if (ratingToRemove != null) Ratings.Remove(ratingToRemove);
            CheckStatus();
        }

        private void EnsureNotClosed()
        {
            if (Status == Status.Closed)
                throw new InvalidOperationException("This blog is closed and cannot be modified.");
        }
    }

    public enum Status
    {
        Draft,
        Published,
        Active,
        Famous,
        Closed
    }
}
