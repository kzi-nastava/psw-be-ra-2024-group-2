using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class UserEquipment: Entity
    {
        public long UserId;
        public long EquipmentId;
        public UserEquipment() { }
    }
}
