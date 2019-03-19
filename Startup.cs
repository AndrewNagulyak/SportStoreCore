using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SportStoreCore.Models;

namespace SportStoreCore
{
    public class Startup
    {
   
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }
        private string _rootpath;
        public Startup(IHostingEnvironment env)
        {
            _rootpath = env.WebRootPath;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IHitCounterService),new HitCounter (_rootpath)));
            services.AddAuthentication().AddGoogle(opts => {

                opts.ClientId = "427325496869-jln91p8k9gi3pv4746ehd7q1bos8smve.apps.googleusercontent.com";
                opts.ClientSecret = "vxuFWFWMPKo7TpiecGo9Oj5M";
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportStoreProducts:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EfOrderRepository>();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admins", policy =>
                {
                    policy.RequireClaim("Admin");
                    policy.RequireClaim("Moderator");
                });
                opt.AddPolicy("Moderator", policy => policy.RequireClaim("Moderator"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(opts => {

                }).AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
          
            services.AddMvc();

            services.AddMemoryCache();
            services.AddSession();
          

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseSession();
            
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("", "Order", new { controller = "Order", action = "List" });
                routes.MapRoute("", "Admin", new { controller = "Admin", action = "Index" });

                routes.MapRoute("", "{controller}", new { controller = "Product", action = "List" });

                routes.MapRoute("", "{category}/Page{productPage}", new {controller = "Product", action = "List"});
                routes.MapRoute("", "Page{productPage}", new { controller = "Product", action = "List" });
                routes.MapRoute("", "{category}", new { controller = "Product", action = "List" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}/{id?}");

            });
        }
    }
}
