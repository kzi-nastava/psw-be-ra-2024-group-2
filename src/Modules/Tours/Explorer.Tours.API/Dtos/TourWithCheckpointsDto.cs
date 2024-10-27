﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class TourWithCheckpointsDto
{
    public TourDto Tour { get; set; }
    public List<CheckpointDto> Checkpoints { get; set; }
}
