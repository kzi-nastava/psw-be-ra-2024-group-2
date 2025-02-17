using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        Console.WriteLine("On Model Creating");
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasColumnName("username");

        Console.WriteLine("On Model Creating username");

        modelBuilder.Entity<User>()
            .Property(u => u.IsActive)
            .HasColumnName("isactive");

        Console.WriteLine("On Model Creating active");

        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .HasColumnName("password");

        Console.WriteLine("On Model Creating pass");

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasColumnName("role");
            

        Console.WriteLine("On Model Creating role");

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .HasColumnName("id");
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        Console.WriteLine("On Model Creating id");

        ConfigureStakeholder(modelBuilder);
    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        Console.WriteLine("Stakeholders start");
        modelBuilder.Entity<Person>()
            .ToTable("people", "stakeholders");  // Explicitly mention schema and table

        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);

        Console.WriteLine("Stakeholders userID");

        modelBuilder.Entity<Person>()
        .Property(p => p.Email)
        .HasColumnName("email");  // Ensure correct case here

        Console.WriteLine("Stakeholders email");

        // Configure other properties similarly
        modelBuilder.Entity<Person>()
            .Property(p => p.Name)
            .HasColumnName("name");

        Console.WriteLine("Stakeholders name");

        modelBuilder.Entity<Person>()
            .Property(p => p.Id)
            .HasColumnName("id");

        Console.WriteLine("Stakeholders id");

        modelBuilder.Entity<Person>()
            .Property(p => p.Surname)
            .HasColumnName("surname");

        Console.WriteLine("Stakeholders surname");

        modelBuilder.Entity<Person>()
            .Property(p => p.UserId)
            .HasColumnName("userid");
        Console.WriteLine("Stakeholders userID");
    }

    

}