using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class AttachedResource : Entity
    {
        public string Url { get; set; }
        public int ResourceId { get; set; }
        public AttachedResourceType Type { get; set; }

        public AttachedResource() { }

        public AttachedResource(int resourceId, string url, AttachedResourceType resourceType)
        {
            ResourceId = resourceId;
            Url = url;
            Type = resourceType;
        }
    }
}
