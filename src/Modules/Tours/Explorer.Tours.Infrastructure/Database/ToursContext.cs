﻿using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourReview> TourReview { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<TourIssueReport> TourIssueReports { get; set; }
    public DbSet<Checkpoint> Checkpoints { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");

        modelBuilder.Entity<TourReview>()
            .HasOne(p => p.Image)
            .WithOne()
            .HasForeignKey<TourReview>(s => s.ImageId)
            .OnDelete(DeleteBehavior.SetNull);


    }

}
