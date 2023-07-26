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
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;






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
           services.AddSingleton<PlanetServices>();
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
           
            app.UseRouting(); //EndpointRoutingMiddleware

            app.UseAuthentication(); // xac dinh danh tinh

            app.UseAuthorization(); // xac thuc quyen truy cap

          
                // endpoints.MapGet("/sayhi", async  (context) =>{
                //        await   context.Response.WriteAsync($"{DateTime.Now}");
                // });
              //  endpoints.MapControllers  cau hinh  tao ra endpoint toi controller, cac enpoint dc dinh nghia truc tiep
              // trong controller thong qua atribut

                // endpoints.MapAreaControllerRoute
            //    endpoints.MapDefaultControllerRoute
            // endpoints.MapAreaControllerRoute  tao ra entpoint den cac controller nam trong erea


                // URL = start-here
                // controller
                // action
                //erea
            // endpoints.MapControllerRoute( 
            //     name :"first", 
            //     pattern :"xemsanpham/{id?}", // 
            // defaults: new {
            //     controller = "First",
            //     action = "ViewProductc",
            //     id = 3
            // dotnet aspnet-codegenerator -h


            // });
             app.UseEndpoints(endpoints =>
            {
                //  endpoints.MapControllerRoute(
                //     name: "first",
                //     pattern: "start-here/{controller}/{action}/{id?}"
                //     // defaults : new {
                //     //     controller =" First",
                //     //     action = "ViewProduct",
                //     //     id =3,
                //    );  
                    //  endpoints.MapControllerRoute(
                    // name: "first",
                    // pattern: "start-here",
                    // defaults : new {
                    //     controller =" First",
                    //     action = "HelloView",
                        // id =3,
                    // });  

                    // [AcceptVerbs("POST", "GET")]
                  //  [Route]
                  //  [HT]
                endpoints.MapControllerRoute(
                    name : "first",
                    pattern: "{url:regex(^((xemsanpham)|(viewproduct))$)}/{id:range(1,4)}",
                    defaults : new {
                        controller = "First",
                        action = "ViewProduct"
                    },
                    constraints: new {
                        // url = new RegexRouteConstraint(@"^((xemsanpham)|(viewproduct))$"),

                        //  id = new RangeRouteConstraint(1,4)
                    }
                );   
                
                // endpoints.MapAreaControllerRoute() // Areas/AreasName/Views/Product/Index.cshtml
               

                   

                // chi thuc hien tai cac controller khong co area

                 endpoints.MapControllerRoute(
                      name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}"); // la route
                    




               




                endpoints.MapRazorPages();

            });
             app.UseStatusCodePages();
            
            
        }
    }
}
