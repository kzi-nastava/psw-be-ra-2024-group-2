using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Club : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ImageId { get; set; }
        public int? OwnerId { get; set; }
        public Club(string name, string description, int? imageId, int ownerId)
        {
            Name = name;
            Description = description;
            ImageId = imageId;
            OwnerId = ownerId;
            Validate();
        }
        public Club()
        {
        }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (ImageId <= 0) throw new ArgumentException("Invalid Image");
            if (OwnerId == 0) throw new ArgumentException("Club must have an Owner");
        }
    }
}
