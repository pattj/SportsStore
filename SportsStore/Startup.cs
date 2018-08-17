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

            /*
             * The AddedScope method specifies that the same object should be used
             * to satisfy related requests for Cart instances. How requests are related
             * can be configured, but by default, it means that any Cart required by components
             * handling the same HTTP request will receive the same object.
             * 
             * If it requires a Cart, the factory method for SessionCart will be called
             * thru the lambda expression. It receives a collection of services that has been
             * registered and pass the collection to the GetCart method of the SessionCart class.
             * The result is that requests for the Cart service will be handled by creating SessionCart 
             * objects, which will serialize themselves as session data when they are modified
             * 
             * the AddSignleton method specifies to MVC that it will use the HttpContextAccessor class when
             * when implementations of the IHttpContextAccessor interface are required.
             * This service is required when access to the current session in the SessionCart
             * 
             * Difference between Transient and Singleton is that Transcient is a new instance while Singleton object is the same object for
             * every request
             * 
             */

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
