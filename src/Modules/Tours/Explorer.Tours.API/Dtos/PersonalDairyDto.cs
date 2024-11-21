using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain.Enums;


namespace Explorer.Tours.API.Dtos
{
    public class PersonalDairyDto
    {

        public int Id { get; set; }

        public long UserId { get; set; }
        public long TourId { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DairyStatus Status { get; set; }


        public List<ChapterDto> chapters { get; set; } = new List<ChapterDto> { };

    }
}
