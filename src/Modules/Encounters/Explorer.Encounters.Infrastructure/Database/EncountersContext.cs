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

        }
    }
}
