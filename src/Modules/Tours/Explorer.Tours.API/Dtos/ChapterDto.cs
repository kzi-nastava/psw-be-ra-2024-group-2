using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.API.Dtos
{
    public class ChapterDto
    {
        public long ChapterId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public long PersonalDairyId { get; set; }
        public Image? Image { get; set; } 
    }
}
