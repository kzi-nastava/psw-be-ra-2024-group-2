using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class EventSubscription : Entity
    {
        public long UserId { get; private set; }
        public EventCategory EventCategory { get; private set; }
        public EventNotificationStatus Status { get; private set; }
    
        public EventSubscription() { }

        public EventSubscription(long userId, EventCategory eventCategory, EventNotificationStatus status)
        {
            UserId = userId;
            EventCategory = eventCategory;
            Status = status;
        }

        public void UpdateStatus(EventNotificationStatus status)
        {
            Status = status;
        }
        public void Read()
        {
            Status = EventNotificationStatus.Read;
        }
        public void UnRead()
        {
            Status = EventNotificationStatus.Unread;
        }




    }
}
