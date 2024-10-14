using System;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class Comment : Entity
{
    public long BlogId { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; }
    public DateTime? LastModifiedAt { get; private set; }

    public Comment(long blogId, string userName, string text)
    {
        BlogId = blogId;
        UserName = userName;
        Text = text;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    private void Validate()
    {
        if (BlogId == 0) throw new ArgumentException("Invalid BlogId");
        if (string.IsNullOrWhiteSpace(UserName)) throw new ArgumentException("Invalid UserName");
        if (string.IsNullOrWhiteSpace(Text)) throw new ArgumentException("Invalid Text");
    }
}
