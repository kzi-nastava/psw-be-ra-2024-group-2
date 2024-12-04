using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain
{
    public class TourSale : Entity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set;  }
        public double DiscountPercentage { get; set; }

        public List<TourSaleTour> Tours { get; set; } = new List<TourSaleTour>();

        public TourSale(string name, DateTime startDate, DateTime endDate, int userId, double discountPercentage)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            UserId = userId;
            DiscountPercentage = discountPercentage;
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null or empty.");

            if (StartDate == null)
                throw new ArgumentNullException(nameof(StartDate), "Start date cannot be null.");

            if (EndDate == null)
                throw new ArgumentNullException(nameof(EndDate), "End date cannot be null.");

            if (DiscountPercentage <= 0.00)
                throw new ArgumentOutOfRangeException(nameof(DiscountPercentage), "Discount percentage must be greater than zero.");
        }

        public TourSaleTour AddTour(TourSaleTour tour)
        {
            Tours.Add(tour);
            return tour;
        }

        public List<TourSaleTour> UpdateTours(List<TourSaleTour> toursChanged)
        {
            Tours = toursChanged;
            return toursChanged;
        }

        public TourSaleTour DeleteTour(TourSaleTour tour)
        {
            Tours.Remove(tour);
            return tour;
        }

    }
}
