﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Data;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Database support
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration["Data:SportsStoreProducts:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();

            // Session support
            services.AddMemoryCache();
            services.AddSession();

            // Shopping cart service
            services.AddScoped(SessionCart.GetCart);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "{category}/Page{productPage:int}",
                    new { Controller = "Product", action = "List" });
                routes.MapRoute(null, "Page{productPage:int}",
                    new { Controller = "Product", action = "List", productPage = 1 });
                routes.MapRoute(null, "{category}",
                    new { Controller = "Product", action = "List", productPage = 1 });
                routes.MapRoute(null, "",
                    new { Controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(null, "{controller}/{action}/{id?}");
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
