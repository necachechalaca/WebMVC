using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webmvc.Services;


namespace webmvc.Controllers
{
    [Area("ProductManager")]
    public class ProductController : Controller
    {
       private readonly  ProdcutServices _productServices;
         private readonly ILogger<FirstController> _logger;

        public ProductController(ILogger<FirstController> logger, ProdcutServices planets){
            _logger = logger;
            _productServices = planets ;
        }
        public IActionResult Index()
        {
             _logger.LogInformation("Hello");
             var product = _productServices.OrderBy(p => p.productName).ToList();
            return View(product);
        }
    }
}