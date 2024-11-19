using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Event : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public EventCategory Category { get; private set; }
        public long? ImageId { get; set; }
        public Image? Image { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Event() { }
        public Event(string name, string description, EventCategory category, double longitude, double latitude, DateTime start, DateTime end)
        {
            Name = name;
            Description = description;
            Category = category;
            Longitude = longitude;
            Latitude = latitude;
            StartDate = start;
            EndDate = end;
            Validate();
        }

        public string GetEventCategoryNormalized => Category switch
        {
            EventCategory.BasketballMatch => "BasketballMatch",
            EventCategory.MusicFestival => "MusicFestival",
            EventCategory.FilmFestival => "FilmFestival",
            EventCategory.FootballMatch => "FootballMatch",
            EventCategory.Concert => "Concert",
            _ => throw new ArgumentException("Category is not valid")
        };

        public EventCategory GetEventCategoryDenormalized(string category) => category switch
        {
            "BasketballMatch" => EventCategory.BasketballMatch,
            "MusicFestival" => EventCategory.MusicFestival,
            "FilmFestival" => EventCategory.FilmFestival,
            "FootballMatch" => EventCategory.FootballMatch,
            "Concert" => EventCategory.Concert,
            _ => throw new ArgumentException("Category is not valid")
        };

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name.");
            if (!Enum.IsDefined(typeof(EventCategory), Category)) throw new ArgumentException("Invalid event category.");
            if (double.IsNaN(Longitude)) throw new ArgumentException("Invalid location.");
            if (double.IsNaN(Latitude)) throw new ArgumentException("Invalid location");
        }
    }
}
