using System;
namespace Explorer.Tours.API.Dtos
{
    public class GuideReportDto
    {
        public long GuideId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<SoldTourDto> SoldTours { get; set; }
        public RatedTourDto BestRatedTour { get; set; }
        public RatedTourDto WorstRatedTour { get; set; }
        public int totalSales { get; set; }

        public string ConvertGuideReportToHtml()
        {
            var reportHtml = $"<h3>Izveštaj o turama za vodiča {this.GuideId} za mesec {this.Month}/{this.Year}:</h3>";

            // Add Sold Tours Information
            reportHtml += "<h4>Održane ture i broj prodatih karata:</h4>";
            if (this.SoldTours != null && this.SoldTours.Count > 0)
            {
                reportHtml += "<ul>";
                foreach (var soldTour in this.SoldTours)
                {
                    reportHtml += $"<li>Tura: {soldTour.TourName}, Broj prodatih karata: {soldTour.SalesCount}</li>";
                }
                reportHtml += "</ul>";
            }
            else
            {
                reportHtml += "<p>Nema prodatih karata za ove ture.</p>";
            }

            // Add Best Rated Tour Information
            if (this.BestRatedTour != null)
            {
                reportHtml += $"<p><strong>Najbolje ocenjena tura:</strong> {this.BestRatedTour.TourName} (Ocena: {this.BestRatedTour.AverageRating}/5, {this.BestRatedTour.RatingCount} ocena)</p>";
            }
            else
            {
                reportHtml += "<p>Nema najbolje ocenjene ture.</p>";
            }

            // Add Worst Rated Tour Information
            if (this.WorstRatedTour != null)
            {
                reportHtml += $"<p><strong>Najgore ocenjena tura:</strong> {this.WorstRatedTour.TourName} (Ocena: {this.WorstRatedTour.AverageRating}/5, {this.WorstRatedTour.RatingCount} ocena)</p>";
            }
            else
            {
                reportHtml += "<p>Nema najgore ocenjene ture.</p>";
            }

            return reportHtml;
        }


    }

    public class SoldTourDto
    {
        public long TourId { get; set; }
        public string TourName { get; set; }
        public int SalesCount { get; set; }
    }

    public class RatedTourDto
    {
        public long TourId { get; set; }
        public string TourName { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}

