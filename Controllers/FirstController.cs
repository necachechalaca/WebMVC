using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using webmvc.Services;

namespace webmvc.Controllers
{


    public class FirstController : Controller
    {
        private readonly ProdcutServices _productServices;
        private readonly ILogger<FirstController> _logger;
         public FirstController(ILogger<FirstController> logger, ProdcutServices productServies)
         {
            _logger = logger;
            _productServices = productServies;
         }
        // this.HttpContext
       // this.Rquest 
        // this.Response
       // this.RouteData
       // this.User
       // this.ModelState
       // this.ViewData
       // this.ViewData
       // this.Url
       // this.TempData
      
        public string Index()
        {
            
            _logger.LogInformation("Index");
            return "day la Index cua First";
        }
        public void Nothing()
        {
            _logger.LogInformation("Nothing Log");
            Response.Headers.Add("Hi","xin chao");
          
        } 
        public IActionResult ReadMe()
        {
            var content =@" xIN CHAO MOI NGUOI
            hehe
            deng io choi game r
            ";
            return Content(content, "text/plain");
        }
        public IActionResult England()
        {
           // Startup.ContentRootPath
           string filePath = Path.Combine(Startup.ContentRootPath, "File", "giogiac.jpg");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            
          return  File(bytes,"text/plain");
        }
         public IActionResult IphonePrice()
        {
            return Json(
                new {
                    productName = "IphoneName",
                    Price = 100

                }
            );
        }
         public IActionResult ChuyenHuong()
        {
            var url = Url.Action("Privacy", "Home");
           return LocalRedirect(url); //url la local khong coo host
        }
        public IActionResult ChuyenHuonga()
        {
            var url = "https://google.com";
           return Redirect(url); //url la local khong coo host
        }
        public IActionResult HelloView(string username)
        {
            if(string.IsNullOrEmpty(username))
            username = "Khaach";
            // View() su dung Razor Engine doc va thi hanh file cshtml(template)
            // Cach 1; Su dung View(Template ) - template la duong dan tuyet doi toi file cs.html
            // TRuyen du lieu tu Controll sang View thiet lap o tham so thu 2 cua phuong thuc view, la model
           return View("xinchao3", username);
           // cach 1 tra ve view
           // cach 2 tra ve Model
        }

        //theo Model
        [AcceptVerbs("POST", "GET")]
        public IActionResult ViewProduct(int? id)
        {
            var product = _productServices.Where(p => p.productId ==id).FirstOrDefault();
            if( product == null){
                return NotFound();

            }
            //View/First/ViewProduct.cshtml(template mac dinh giong ten action(ViewProduct))    
            return View(product);
        }

           // theo ViewData


         public IActionResult ViewProducta(int? id)
        {
            var product = _productServices.Where(p =>p.productId == id).FirstOrDefault();
            if( product == null){
                return NotFound();
            }               
            this.ViewData["product"] = product;
            // View/First/Viewproduct2.cshtml
            return View("ViewProduct2");
        }

        //ViewBag
         public IActionResult ViewProductb(int? id)
        {
            var product = _productServices.Where(p =>p.productId == id).FirstOrDefault();
            if( product == null){
                return NotFound();
            }
            ViewBag.product =product;
            return View("ViewProduct3");
        }
        //TempDATA

         public IActionResult ViewProductc(int? id)
        {
            var product = _productServices.Where(p =>p.productId == id).FirstOrDefault();
            if( product == null)
            {
                TempData["Thong bao"] = "San pham ban truy cap khong co";
                return Redirect(Url.Action("Index", "Home"));
            }
             this.ViewData["product"] = product;
            return View("ViewProduct2");
        }

    }
}