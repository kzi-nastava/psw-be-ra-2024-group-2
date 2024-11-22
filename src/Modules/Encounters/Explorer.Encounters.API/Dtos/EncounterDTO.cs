using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}