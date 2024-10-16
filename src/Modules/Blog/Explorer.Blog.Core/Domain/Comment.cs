using System;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class Comment : Entity
{
    public long BlogId { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; }
    public DateTime? LastModifiedAt { get; private set; }

    public Comment(long blogId, string text)
    {
        BlogId = blogId;
        Text = text;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    public void UpdateLastModifiedAt()
    {
        LastModifiedAt = DateTime.UtcNow;
    }
    private void Validate()
    {
        if (BlogId == 0) throw new ArgumentException("Invalid BlogId");
        if (string.IsNullOrWhiteSpace(Text)) throw new ArgumentException("Invalid Text");
    }
}
