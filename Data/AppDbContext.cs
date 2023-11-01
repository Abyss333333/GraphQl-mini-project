using Microsoft.EntityFrameworkCore;
using CommanderGQL.Models;

namespace CommanderGQL.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany( p => p.Commands)
                .WithOne(e => e.Platform!)
                .HasForeignKey(e => e.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(e => e.Platform)
                .WithMany( e => e.Commands)
                .HasForeignKey( e => e.PlatformId);
        }
    }
}