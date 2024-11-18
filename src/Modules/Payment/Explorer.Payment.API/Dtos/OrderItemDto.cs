using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class OrderItemDto
{
    public string TourName { get; set; }
    public double Price { get; set; }
    public long TourId { get; set; }
    public long UserId { get; set; }
}