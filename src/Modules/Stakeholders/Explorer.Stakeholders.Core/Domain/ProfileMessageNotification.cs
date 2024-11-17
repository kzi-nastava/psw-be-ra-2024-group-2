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
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public ProfileMessageNotificationStatus Status { get; set; }
        public long ProfileMessageId { get; set; }
        public ProfileMessage ProfileMessage { get; set; }

        public ProfileMessageNotification() { }

        public ProfileMessageNotification(long recipientId, long senderId) 
        {
            RecipientId = recipientId;
            SenderId = senderId;
        }

        public ProfileMessageNotification(long recipientId, long senderId, ProfileMessageNotificationStatus status)
        {
            RecipientId = recipientId;
            SenderId = senderId;
            Status = status;
        }

        public ProfileMessageNotification(long recipientId, long senderId, ProfileMessageNotificationStatus status, int messageId)
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
