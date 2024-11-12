using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProfileMessageDto
    {
        public long SenderId { get; private set; }
        public long RecipientId { get; set; }
        public string Text { get; set; }
        public string Resource { get; set; }
        public DateTime SentAt { get; set; }
    }
}
