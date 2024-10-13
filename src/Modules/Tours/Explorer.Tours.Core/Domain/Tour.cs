using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Tour  : Entity
    {


        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TourDifficulty Difficulty { get; set; }
        public TourTag Tag { get; set; }                   
        public TourStatus Status { get; set; }
        public double Price { get; set; }
        

       public Tour(int userId, string name, string description, TourDifficulty difficulty, TourTag tag, TourStatus status, double price)
       {
            UserId = userId;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Tag = tag;
            Status = status;
            Price = price;
       }
    }

    public enum TourStatus
    {
        Draft,
        Published,
        Archived
    }


    public enum TourTag 
    {
        Adventure,
        Relaxation,
        Historical,
        Cultural,
        Nature
    }

    public enum TourDifficulty
    {
        Easy,
        Moderate,
        Hard
    }
}
