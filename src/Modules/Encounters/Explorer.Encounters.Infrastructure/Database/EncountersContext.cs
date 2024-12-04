using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {
        public DbSet<Encounter> encounters { get; set; }
        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("encounters");
           

            ConfigureEncounters(modelBuilder);
        }
        private static void ConfigureEncounters(ModelBuilder modelBuilder)
        {
            //implementing IS-a hierarchy in er
            modelBuilder.Entity<Encounter>()
                .ToTable("Encounters")  
                .HasDiscriminator<string>("EncounterType")  
                .HasValue<SocialEncounter>("Social")  
                .HasValue<HiddenLocationEncounter>("HiddenLocation") 
                .HasValue<MiscEncounter>("Misc");  
        }
    }
}
