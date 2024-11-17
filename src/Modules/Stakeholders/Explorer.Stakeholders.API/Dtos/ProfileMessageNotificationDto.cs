using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProfileMessageNotificationDto
    {
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public ProfileMessageNotificationStatus Status { get; private set; }
        public long ProfileMessageId { get; private set; }
    }
}
