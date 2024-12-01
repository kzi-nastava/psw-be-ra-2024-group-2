using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain;

public class Coupon : Entity
{
    public string Code { get; set; } 
    public int DiscountPercentage { get; set; } 
    public long TourId { get; set; } 
    public long AuthorId { get; set; }
    public Boolean AllToursDiscount { get; set; }

    public Coupon(long authorId, long tourId, int discountPercentage, Boolean allToursDiscount)
    {
        AuthorId = authorId;
        TourId = tourId;
        Code = GenerateCouponCode();
        DiscountPercentage = discountPercentage;
        AllToursDiscount = allToursDiscount;
    }

    private string GenerateCouponCode()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] code = new char[8];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = chars[random.Next(chars.Length)];
        }
        return new string(code);
    }
}
