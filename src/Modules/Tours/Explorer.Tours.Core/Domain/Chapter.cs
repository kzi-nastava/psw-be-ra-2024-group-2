using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Chapter
    {
        public long ChapterId { get; private set; }
        public string Title { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Text { get; private set; }
        public long PersonalDairyId { get; set; }

        private Chapter() { }

        public Chapter(string title, string text)
        {
            Title = title;
            Text = text;
            CreatedAt = DateTime.UtcNow;
        }
        
        public void UpdateText(string newText)
        {
            Text = newText;
        }
    }
}

