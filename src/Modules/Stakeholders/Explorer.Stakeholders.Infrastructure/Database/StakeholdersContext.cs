﻿using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<RatingApplication> RatingsApplication { get; set; }
    public DbSet<ProfileMessage> ProfileMessages { get; set; }
    public DbSet<ProfileMessageNotification> ProfileMessageNotifications { get; set; }

    public DbSet<FAQ> FAQs { get; set; }

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

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<Person>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Person>()
                .Property(t => t.TouristPosition).HasColumnType("jsonb");
    }
}