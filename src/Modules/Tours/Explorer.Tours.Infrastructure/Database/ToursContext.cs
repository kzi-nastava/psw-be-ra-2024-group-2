using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourReview> TourReview { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<TourObject> Objects { get; set; }
    public DbSet<TourIssueReport> TourIssueReports { get; set; }
    public DbSet<TourIssueComment> TourIssueComments { get; set; }
    public DbSet<TourIssueNotification> TourIssueNotifications { get; set; }
    public DbSet<ClubInvite> ClubInvites { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Checkpoint> Checkpoints { get; set; }
    public DbSet<TourPreference> TourPreferences { get; set; }
    public DbSet<TourPreferenceTag> PreferenceTags { get; set; }
    public DbSet<TourDurationByTransport> TourDurationByTransports { get; set; }

    public DbSet<TourExecution> TourExecutions { get; set; }

    public DbSet<TourExecutionCheckpoint> TourExecutionCheckpoints { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Entity<Club>()
            .Property(c => c.ImageId)
            .IsRequired(false);

        modelBuilder.Entity<Club>()
            .Property(c => c.OwnerId)
            .IsRequired();

        modelBuilder.Entity<TourReview>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<TourReview>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.TourDurationByTransports)
            .WithOne(t => t.Tour)
            .HasForeignKey(t => t.TourId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TourExecution>()
            .HasMany(te => te.tourExecutionCheckpoints)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
       
        modelBuilder.Entity<Tour>()
            .HasMany(t => t.TourIssueReports)
            .WithOne(t => t.Tour)
            .HasForeignKey(t => t.TourId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TourIssueReport>()
            .HasMany(t => t.TourIssueComments)
            .WithOne(t => t.TourIssueReport)
            .HasForeignKey(t => t.TourIssueReportId)
            .OnDelete(DeleteBehavior.Cascade);

     
        modelBuilder.Entity<TourIssueReport>()
            .HasMany(t => t.TourIssueNotifications)
            .WithOne(t => t.TourIssueReport)
            .HasForeignKey(t => t.TourIssueReportId)
            .OnDelete(DeleteBehavior.Cascade);
       
    }
}