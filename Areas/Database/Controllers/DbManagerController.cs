using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webmvc.Models;
using Microsoft.EntityFrameworkCore;

namespace webmvc.Areas.Database.Controllers
{


    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManagerController : Controller
    {

        private readonly AppDbContext _appdbContext;

        public DbManagerController(AppDbContext appDbContext){
            _appdbContext = appDbContext;
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

    }
}