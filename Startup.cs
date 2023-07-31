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
using webmvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;







namespace webmvc
{
    public class Startup
    {
        public static string ContentRootPath { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddDbContext<AppDbContext>(option =>
            {
                var connection = Configuration.GetConnectionString("MyWeb");
               option.UseSqlServer(connection);
            });


            services.AddOptions();
            var mailSetting = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSetting);
            services.AddSingleton<IEmailSender, SendMailService>();


           
            // Dang Ky Identity
            // services.AddIdentity<AppUser, IdentityRole>()
            //         .AddEntityFrameworkStores<AppDbContext>()
            //         .AddDefaultTokenProviders();

              services.AddDefaultIdentity<AppUser>() 
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<RazorViewEngineOptions>(option =>
            {
                //View/Controller/Action.cshtml
                //MyView/Controller//Action.cshtml
                // {0} ten action
                // {1} ten controller
                // {2} ten ares

                option.ViewLocationFormats.Add("MyView/{1}/{0}.cshtml" + RazorViewEngine.ViewExtension);

            });
            // services.AddSingleton<ProdcutServices>();
            services.AddSingleton<ProdcutServices, ProdcutServices>();
            services.AddSingleton<PlanetServices>();
            services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

            });
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
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
                   name: "first",
                   pattern: "{url:regex(^((xemsanpham)|(viewproduct))$)}/{id:range(1,4)}",
                   defaults: new
                   {
                       controller = "First",
                       action = "ViewProduct"
                   },
                   constraints: new
                   {
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
