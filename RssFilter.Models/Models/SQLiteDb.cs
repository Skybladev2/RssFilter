using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFilter.Models
{
    public class SQLiteDb : DbContext
    {
        public SQLiteDb() : base() { }
        public SQLiteDb(DbContextOptions<SQLiteDb> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db\\mydb.db;");
        }
    }
}
