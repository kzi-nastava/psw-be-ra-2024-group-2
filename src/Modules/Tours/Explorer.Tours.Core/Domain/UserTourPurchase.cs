using System;
using System.Collections.Generic;

namespace Explorer.Tours.Core.Domain
{
    // Enum to define the purchase status
    public enum PurchaseStatus
    {
        Active = 1,
        Cancelled = 2
    }

    public class UserTourPurchase
    {
        // Properties
        public long UserId { get; set; } // ID of the user who made the purchase
        public long TourId { get; set; } // ID of the tour purchased
        public DateTime Date { get; set; } // Date when the purchase was made
        public PurchaseStatus Status { get; set; } // Status of the purchase (Active or Cancelled)        
        public string UserEmail { get; set; }

        // Constructor
        public UserTourPurchase(long userId, long tourId, DateTime date)
        {
            UserId = userId;
            TourId = tourId;
            Date = date;
            Status = PurchaseStatus.Active;
            UserEmail = "sergej.vlaskalic@gmail.com";
        }

        public UserTourPurchase(long userId, long tourId, DateTime date, string email)
        {
            UserId = userId;
            TourId = tourId;
            Date = date;
            Status = PurchaseStatus.Active;
            UserEmail = email;
        }

        // Method to update the status
        public void UpdateStatus(PurchaseStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
