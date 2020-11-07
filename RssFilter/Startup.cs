using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RssFilter.Models;

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
            services.AddDbContext<PostgresDB>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")), ServiceLifetime.Singleton);
    
            services.AddControllers();

            new Thread(DbThreadProc).Start();
        }

        public static void DbThreadProc()
        {
            while (newDataAddedEvent.WaitOne())
            {
                newDataAddedEvent.Reset();
                var batch = new HashSet<int>();
                while (queue.Count > 0)
                {
                    var item = queue.Dequeue();
                    if (!batch.Contains(item))
                    {
                        System.Diagnostics.Debug.WriteLine("             Put: " + item);
                        batch.Add(item);
                    }
                }
                UpdateDb(batch);
            }

        }

        private static void UpdateDb(HashSet<int> batch)
        {
            System.Diagnostics.Debug.WriteLine("Writing to database");
            Thread.Sleep(5000);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
