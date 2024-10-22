using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ClubInvite> ClubInvites { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<RatingApplication> RatingsApplication { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        ConfigureStakeholder(modelBuilder);
    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        modelBuilder.Entity<Club>()
        .Property(c => c.ImageId)
        .IsRequired(false);

        modelBuilder.Entity<Club>()
        .Property(c => c.OwnerId)
        .IsRequired();

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<Person>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}