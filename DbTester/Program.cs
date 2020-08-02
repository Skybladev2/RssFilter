using Microsoft.EntityFrameworkCore;
using RssFilter.Models;
using System;

namespace DbTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgresDB>();
            optionsBuilder.UseNpgsql("User ID=postgres;Password=1;Server=localhost;Port=5432;Database=RssFilter;Pooling=true;");
            using (var _context = new PostgresDB(optionsBuilder.Options))
            {

            }
        }
    }
}
