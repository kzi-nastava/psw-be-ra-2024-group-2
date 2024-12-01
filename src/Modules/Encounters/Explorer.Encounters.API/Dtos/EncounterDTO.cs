using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterDto
    {
        public long Id { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;  
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public List<int> TouristIds { get; set; } = new List<int>();
    }
}