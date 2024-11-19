using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class FAQ : Entity
    {
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? LastUpdatedDate { get; private set; }

        public FAQ(string question, string answer, DateTime createdDate)
        {
            Question = question;
            Answer = answer;
            CreatedDate = createdDate;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Question))
                throw new ArgumentException("Invalid Question. Question cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(Answer))
                throw new ArgumentException("Invalid Answer. Answer cannot be null, empty, or whitespace.");
            if (CreatedDate == default)
                throw new ArgumentException("Invalid CreatedDate. CreatedDate must be a valid date.");
        }

        public void UpdateQuestion(string question)
        {
            Question = question;
        }

        public void UpdateAnswer(string answer)
        {
            Answer = answer;
        }

        public void UpdateCreatedDate(DateTime createdDate)
        {
            CreatedDate = createdDate;
        }

        public void UpdateLastUpdatedDate(DateTime? lastUpdatedDate)
        {
            LastUpdatedDate = lastUpdatedDate;
        }
    }
}
