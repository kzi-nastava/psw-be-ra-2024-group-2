using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourIssueComment : ValueObject
    {
        public long UserId { get; private set; }
        public long TourIssueId { get; private set; }
        public string Comment { get; private set; }
        public DateTime PublishedAt { get; private set; }

        [JsonConstructor]
        public TourIssueComment(long userId, long tourIssueId, string comment, DateTime publishedAt)
        {
            UserId = userId;
            TourIssueId = tourIssueId;
            Comment = comment;
            PublishedAt = publishedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return TourIssueId;
            yield return Comment;
            yield return PublishedAt;
        }

        public void UpdateComment(string comment)
        {
            Comment = comment;
        }
    }
}
