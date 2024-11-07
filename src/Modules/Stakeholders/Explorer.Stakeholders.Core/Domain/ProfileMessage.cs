using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ProfileMessage : Entity
    {
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public string Text { get; set; }
        //public AttachedResource Resource { get; set; }
        public string Resource { get; set; }
        public DateTime SentAt { get; set; }
        public ProfileMessage() { }
        public ProfileMessage(long senderId, long recipientId, string text, string resource)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Text = text;
            Resource = resource;
            SentAt = DateTime.Now;
            Validate();
        }

        private void Validate()
        {
            if (Text.Length > 280)
                throw new ArgumentException("Message text cannot exceed 280 characters.");
        }
    }
}
