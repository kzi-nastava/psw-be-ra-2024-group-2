using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<KeyPoint> KeyPoints { get; set; }
    public DbSet<UserTourPurchase> Purchases { get; set; }
    public DbSet<TourRate> TourRates { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        ConfigureTour(modelBuilder);
        ConfigureKeyPoints(modelBuilder);
        ConfigurePurchase(modelBuilder);
        ConfigureTourRate(modelBuilder);
    }

    private static void ConfigureTour(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>()
            .Property(t => t.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Name)
            .HasColumnName("name");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Description)
            .HasColumnName("description");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Difficulty)
            .HasColumnName("difficulty");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Category)
            .HasColumnName("category");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Price)
            .HasColumnName("price");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Date)
            .HasColumnName("date");

        modelBuilder.Entity<Tour>()
            .Property(t => t.GuideId)
            .HasColumnName("guideid");

        modelBuilder.Entity<Tour>()
            .Property(t => t.Status)
            .HasColumnName("status");
    }

    private static void ConfigureKeyPoints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyPoint>()
            .ToTable("keypoints", "public");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Id)
            .HasColumnName("id");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Name)
            .HasColumnName("name");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Description)
            .HasColumnName("description");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Latitude)
            .HasColumnName("latitude");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Longitude)
            .HasColumnName("longitude");

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.TourId)
            .HasColumnName("tourid");
    }

    
    private static void ConfigurePurchase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTourPurchase>()
            .ToTable("UserTourPurchase", "public");  // Ensure the table name and schema are correct

        modelBuilder.Entity<UserTourPurchase>()
        .HasKey(utp => new { utp.UserId, utp.TourId });

        modelBuilder.Entity<UserTourPurchase>()
            .Property(p => p.UserId)
            .HasColumnName("userId");

        modelBuilder.Entity<UserTourPurchase>()
            .Property(p => p.TourId)
            .HasColumnName("tourId");

        modelBuilder.Entity<UserTourPurchase>()
            .Property(p => p.Date)
            .HasColumnName("Date");

        modelBuilder.Entity<UserTourPurchase>()
            .Property(p => p.Status)
            .HasColumnName("Status");

        modelBuilder.Entity<UserTourPurchase>()
            .Property(p => p.UserEmail)
            .HasColumnName("UserEmail");
    }

    private static void ConfigureTourRate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.Id)
            .HasColumnName("id");

        modelBuilder.Entity<TourRate>()
            .HasKey(tr => tr.Id);  // Setting the primary key for TourRate

        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.TourId)
            .HasColumnName("tour_id");

        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.TouristId)
            .HasColumnName("tourist_id");

        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.Rating)
            .HasColumnName("rating");

        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.Comment)
            .HasColumnName("comment");

        modelBuilder.Entity<TourRate>()
            .Property(tr => tr.TouristUsername)
            .HasColumnName("TouristUsername");
    }




}