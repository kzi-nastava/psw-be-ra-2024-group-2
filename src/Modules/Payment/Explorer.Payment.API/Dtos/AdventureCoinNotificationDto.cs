using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos
{
    public class ActivationCoinNotificationDto
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public bool Status { get; set; }
        public DateTime SentAt { get; set; }
        public string Message { get; set; } = "You have received Adventure Coins!";
    }

}
