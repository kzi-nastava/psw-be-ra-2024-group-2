﻿using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain;

public class TourPurchaseToken : Entity
{
    public long UserId { get; private set; }
    public long TourId { get; private set; }
    public double Price { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public TourPurchaseToken(long userId, long tourId, double price, DateTime purchaseDate)
    {
        UserId = userId;
        TourId = tourId;
        Price = price;
        PurchaseDate = purchaseDate;
    }
}
