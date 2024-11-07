using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ProfileMessageNotification : Entity
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public ProfileMessageNotificationStatus Status { get; private set; }
        public long ProfileMessageId { get; private set; }
        public ProfileMessage ProfileMessage { get; private set; }

        public ProfileMessageNotification(int recipientId, int senderId, ProfileMessageNotificationStatus status, int messageId)
        {
            RecipientId = recipientId;
            SenderId = senderId;
            Status = status;
            ProfileMessageId = messageId;
        }
        public void UpdateStatus(ProfileMessageNotificationStatus status)
        {
            Status = status;
        }
        public void Read()
        {
            Status = ProfileMessageNotificationStatus.Read;
        }
        public void UnRead()
        {
            Status = ProfileMessageNotificationStatus.Unread;
        }
    }
}
