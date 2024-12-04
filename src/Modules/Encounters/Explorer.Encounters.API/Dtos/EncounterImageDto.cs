using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterImageDto
    {
        public required string Data { get; set; }
        public required DateTime UploadedAt { get; set; }
        public required string MimeType { get; set; }
    }
}
