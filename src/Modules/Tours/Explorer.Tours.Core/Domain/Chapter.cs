using System;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Chapter
    {
        public long ChapterId { get; private set; }
        public string Title { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Text { get; private set; }
        public long PersonalDairyId { get; set; }
        public Image? Image { get; set; } // Opciono svojstvo za sliku

        private Chapter() { }

        public Chapter(string title, string text)
        {
            Title = title;
            Text = text;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateTextAndTitle(string newText, string title)
        {
            Text = newText;
            Title = title;
        }

        public Image AddImage(Image image)
        {
            EnsureImageNotSet();
            Image = image;
            return image;
        }

        public void UpdateImage(Image image)
        {
            EnsureImageExists();
            Image = image;
        }

        public void RemoveImage()
        {
            EnsureImageExists();
            Image = null;
        }

        private void EnsureImageNotSet()
        {
            if (Image != null)
                throw new InvalidOperationException("Chapter already has an image.");
        }

        private void EnsureImageExists()
        {
            if (Image == null)
                throw new InvalidOperationException("Chapter does not have an image to modify or remove.");
        }
    }
}
