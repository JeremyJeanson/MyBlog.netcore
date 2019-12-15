using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.Engine;
using MyBlog.Engine.Data;
using MyLib.Web;

namespace MyBlog
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
            // Add MVC with the minimum services
            IMvcCoreBuilder mvc = services.AddMvcCore()
                .AddAuthorization()
                .AddViews()
                .AddRazorViewEngine()
                .AddCacheTagHelper()
                .AddDataAnnotations();

            // Add BlogEngine
            services.AddMyBlogEngine(Configuration, mvc);

            services.AddMylibWeb();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Use MVC
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Use BlogEngine
            app.UseMyBlogEngine();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Post}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    "withTitle",
                    "{controller=Post}/{action=Details}/{id}/{title?}");
            });
        }
    }
}
