using System;
namespace Explorer.Tours.API.Dtos
{
	public class KeyPointDto
	{
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}

