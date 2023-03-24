using CsGOStateEmitter.Entities;
using CsGOStateEmitter.MappingConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CsGOStateEmitter
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Result> Result { get; set; }
        public DbSet<PlayerStats> Stats { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity(EntitiesMapping.ConfigureMatch());
            modelBuilder.Entity(EntitiesMapping.ConfigureDiscordUser());
            modelBuilder.Entity(EntitiesMapping.ConfigurePlayer());
            modelBuilder.Entity(EntitiesMapping.ConfigureAdminBot());

            // Tabelas utilizadas pelo anticheating
            modelBuilder.Entity(EntitiesMapping.ConfigurePlayers()); 
            modelBuilder.Entity(EntitiesMapping.ConfigureGameScreemShots());
        }
    }
}
