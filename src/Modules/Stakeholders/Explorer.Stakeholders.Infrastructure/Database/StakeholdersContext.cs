using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<RatingApplication> RatingsApplication { get; set; }
    public DbSet<TouristEquipment> TouristEquipments { get; set; }
    public DbSet<ProfileMessage> ProfileMessages { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<TouristEquipment>()
       .HasKey(ue => new { ue.UserId, ue.EquipmentId }); 

        modelBuilder.Entity<TouristEquipment>()
            .Property(ue => ue.UserId)
            .IsRequired(); 

        modelBuilder.Entity<TouristEquipment>()
            .Property(ue => ue.EquipmentId)
            .IsRequired();
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

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<Person>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Person>()
                .OwnsOne(p => p.TouristPosition, tp =>
                {
                    tp.Property(t => t.Latitude).HasColumnName("Latitude");
                    tp.Property(t => t.Longitude).HasColumnName("Longitude");
                });
    }
}