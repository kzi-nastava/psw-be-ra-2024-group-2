using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class UserLevel : Entity
    {
        public long UserId { get; set; }

        public int Level { get; set; }

        public int Xp { get; set; }

        public UserLevel(long userId, int level, int xp)
        {
            this.UserId = userId;
            this.Level = level;
            this.Xp = xp;
        }

        public UserLevel()
        {
        }


    }
}
