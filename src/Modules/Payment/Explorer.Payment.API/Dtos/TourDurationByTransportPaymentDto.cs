using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class TourDurationByTransportPaymentDto
{
    public string Transport { get; set; } = string.Empty;
    public double Duration { get; set; }
}
