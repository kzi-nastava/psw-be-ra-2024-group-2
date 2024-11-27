using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain
{
    public class PersonalDairy : Entity
    {

        public long TourExecutionId { get; private set; }
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public string Title { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ClosedAt { get; private set; }

        public List<Chapter> Chapters = new List<Chapter>();
        //public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();

        public PersonalDairy() { }

        public PersonalDairy(long tourExecutionId,long userId, long tourId, string title)
        {
            TourExecutionId = tourExecutionId;
            UserId = userId;
            TourId = tourId;
            Title = title;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddChapter(string title, string text, Image? image = null)
        {
            var chapter = new Chapter(title, text);

            if (image != null)
            {
                chapter.AddImage(image);
            }

            Chapters.Add(chapter);
        }

        public void RemoveChapter(long chapterId)
        {
            var chapter = Chapters.FirstOrDefault(c => c.ChapterId == chapterId);
            if (chapter != null)
            {
                Chapters.Remove(chapter);
            }
        }
        public void UpdateChapter(long chapterId, string newText, string newTitle)
        {
            var chapter = Chapters.FirstOrDefault(c => c.ChapterId == chapterId);
            if (chapter != null)
            {
                 chapter.UpdateTextAndTitle(newText, newTitle);      
            }
        }
    }
}

