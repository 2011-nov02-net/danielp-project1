using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyStore.DataModel;
using Serilog.Extensions.Logging;

namespace MyStore.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            
            services.AddDbContext<MyStoreDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));

            services.AddTransient<IDbRepository, DbRepositorySingleConnection>();
      
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // from https://www.c-sharpcorner.com/article/add-file-logging-to-an-asp-net-core-mvc-application/
            // which is based off https://nblumhardt.com/2016/10/aspnet-core-file-logger/
            // Takes advantage of dependency injection to do easy file logging
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                   name: "default-customer",
                   pattern: "/Customer/{action=Choose}/{searchString?}");


                //will default to home index when no route is found.
                endpoints.MapFallbackToController(
                        action: "Index",
                        controller: "Home"                      
                    );

            });
        }
    }
}
