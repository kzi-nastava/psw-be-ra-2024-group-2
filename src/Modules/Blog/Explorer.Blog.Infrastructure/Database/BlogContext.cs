using Explorer.Blog.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Blog.Infrastructure.Database;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Explorer.Blog.Core.Domain.Blog> Blogs { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blog");

        modelBuilder.Entity<Explorer.Blog.Core.Domain.Blog>()
          .Property(b => b.Ratings)
          .HasColumnType("jsonb");

        modelBuilder.Entity<Explorer.Blog.Core.Domain.Blog>()
            .HasMany(b => b.Images)
            .WithOne()
            .HasForeignKey("BlogId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Explorer.Blog.Core.Domain.Blog>()
            .HasMany(b => b.Comments)
            .WithOne()
            .HasForeignKey("BlogId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}