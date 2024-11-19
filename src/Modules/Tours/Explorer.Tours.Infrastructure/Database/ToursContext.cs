using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain;
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
    public DbSet<TourExecution> TourExecutions { get; set; }
    public DbSet<TouristEquipment> TouristEquipments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<PersonalDairy> PersonalDairies { get; set; }
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

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Checkpoints)
            .WithMany(c => c.Tours);

        modelBuilder.Entity<TourReview>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<TourReview>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Event>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<Event>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<TourExecution>()
            .Property(te => te.TourExecutionCheckpoints)
            .HasColumnType("jsonb");

        modelBuilder.Entity<Tour>()
            .Property(t => t.TourDurationByTransports)
            .HasColumnType("jsonb");


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

        modelBuilder.Entity<TouristEquipment>()
            .Property(ue => ue.UserId)
            .IsRequired();

        modelBuilder.Entity<TouristEquipment>()
            .Property(ue => ue.EquipmentId)
            .IsRequired();

    }
}