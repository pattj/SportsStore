using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SportsStore
{
    public class Startup
    {
        //Receive DB config from 'appsettings.json'
        public Startup(IConfiguration configuration) =>
        Configuration = configuration;

        //Config that was received from constructor
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {
            //Setting up services provided by EF Core.
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
            Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddMvc();
            
            // For storing detail of a user's cart in a session state (when data is stored at the server 
             // and associated with the series of request made by a  user.
            services.AddMemoryCache(); //Set up the in-memory data store (used for simplicity, will dissapear when app is stop )
            services.AddSession(); //registers the services used to access session data 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession(); // Automatically enable session state for the app and automatically associate request 
                               // with sessions when they arrived from the client.
            app.UseMvc(routes => {

                routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List" }
                );

                routes.MapRoute(
                name: null,
                template: "Page{productPage:int}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                }
                );
                routes.MapRoute(
                name: null,
                template: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                }
                );
                routes.MapRoute(
                name: null,
                template: "",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            
        });
            SeedData.EnsurePopulated(app);
        }
    }
}
