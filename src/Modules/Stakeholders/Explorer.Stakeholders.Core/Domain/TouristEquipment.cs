using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class TouristEquipment: Entity
    {
        public long UserId;
        public long EquipmentId;
      public TouristEquipment() { }
      public TouristEquipment(long userId, long equipmentId)
        {
            UserId = userId;
            EquipmentId = equipmentId;
        }
    }
}
