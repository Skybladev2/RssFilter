
using Microsoft.EntityFrameworkCore;

namespace RssFilter.Models
{
    public class PostgresDB : DbContext
    {
        public PostgresDB(DbContextOptions<PostgresDB> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")  
                   .Entity<Feed>()
                   .Property(e => e.Id)
                   .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.HasPostgresExtension("uuid-ossp")
                   .Entity<Keyword>()
                   .Property(e => e.Id)
                   .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.HasPostgresExtension("uuid-ossp")
                   .Entity<Post>()
                   .Property(e => e.Id)
                   .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.Link)
                .IsUnique();
        }
    }
}
