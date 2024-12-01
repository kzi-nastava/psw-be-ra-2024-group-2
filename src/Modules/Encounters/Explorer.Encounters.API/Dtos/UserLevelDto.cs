using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class UserLevelDto
    {

        public long Id { get; set; }
        public long UserId { get; set; }

        public int Level { get; set; }

        public int Xp { get; set; }
    }
}
