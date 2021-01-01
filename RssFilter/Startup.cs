using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssFilter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RssFilter
{
    public class Startup
    {
        public static ManualResetEvent newDataAddedEvent = new ManualResetEvent(false);
        public static Queue<int> queue = new Queue<int>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<PostgresDB>(opt =>
            //opt.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")), ServiceLifetime.Singleton);
            services.AddDbContext<SQLiteDb>(opt =>
            opt.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));

            services.AddControllers();
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Debug.WriteLine("Start");
            Console.WriteLine("Start");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
