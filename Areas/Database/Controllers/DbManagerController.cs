using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webmvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using webmvc.Data;

namespace webmvc.Areas.Database.Controllers
{


    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManagerController : Controller
    {

        private readonly AppDbContext _appdbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManagerController(AppDbContext appDbContext,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager ){
            _appdbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
       
         [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMassege {get;set;}


        [HttpPost]
         public async Task<IActionResult> DeleteDbAsync()
        {
           var success = await _appdbContext.Database.EnsureDeletedAsync();
           StatusMassege = success ? "Xoa database thanh cong" : " khong xoa duoc";
           return RedirectToAction(nameof(Index));
        }

        [HttpPost]
         public async Task<IActionResult> Migrate()
        {
           await _appdbContext.Database.MigrateAsync();

           StatusMassege = " Tao database thanh conhg";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SeedDataAsync()
        {
            var roleNames = typeof(RoleName).GetFields().ToList();
            foreach (var r in roleNames)
            {
               var rolename =  (string)r.GetRawConstantValue();
               var rfound = await _roleManager.FindByNameAsync(rolename);
               if(rfound == null)
               {
                await _roleManager.CreateAsync(new IdentityRole(rolename));
               }
            }

            // admin , pass = admin123, admin@example.com
            var useradmin = await _userManager.FindByNameAsync("admin");
            if(useradmin == null)
            {
                useradmin = new AppUser(){
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(useradmin, "admin123");
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);
                
            }
            StatusMassege = " Vua seedData";
            return RedirectToAction("Index");
        }

    }
}