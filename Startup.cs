using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using webmvc.Services;



namespace webmvc
{
    public class Startup
    {
        public static string ContentRootPath {get;set;}
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<RazorViewEngineOptions>(option =>{
                //View/Controller/Action.cshtml
                //MyView/Controller//Action.cshtml
                // {0} ten action
                // {1} ten controller
                // {2} ten ares
             
                option.ViewLocationFormats.Add("MyView/{1}/{0}.cshtml" + RazorViewEngine.ViewExtension);

            });
           // services.AddSingleton<ProdcutServices>();
           services.AddSingleton<ProdcutServices,ProdcutServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // xac dinh danh tinh

            app.UseAuthorization(); // xac thuc quyen truy cap

            app.UseEndpoints(endpoints =>
            {


                // truy van Rout enpoint "{controller=Home}/{action=Index}/{id?}");
                // tim den Controller Home va tim den phuong thuc Index

                //First/Index
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapRazorPages();
               
            });
        }
    }
}
