using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourExecutionCheckpointDto
    {
        public int Id { get; set; }
        public long CheckpointId { get;  set; }
        public DateTime ArrivalAt { get;  set; }
    }
}
