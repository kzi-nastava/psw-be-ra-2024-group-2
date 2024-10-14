using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            Validate();
       }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid UserId");
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (Difficulty != TourDifficulty.Easy && Difficulty !=TourDifficulty.Moderate && Difficulty != TourDifficulty.Hard) throw new ArgumentException("Invalid Difficulty");
            if(Status != TourStatus.Draft && Status != TourStatus.Archived && Status != TourStatus.Published) throw new ArgumentException("Invalid Status");
            if(Tag != TourTag.Adventure && Tag != TourTag.Relaxation && Tag != TourTag.Historical && Tag != TourTag.Cultural && Tag != TourTag.Nature) throw new ArgumentException("Invalid Tag");
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
