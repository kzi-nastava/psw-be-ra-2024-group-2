using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public sealed class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Radius { get; set; }
}
