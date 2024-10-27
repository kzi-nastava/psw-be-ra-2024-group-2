using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TouristPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        TouristPosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /*TouristPosition CreateNewPosition(TouristPosition tp)
        {
            return new TouristPosition(tp.Latitude, tp.Longitude);
        }*/
        public void UpdatePosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
