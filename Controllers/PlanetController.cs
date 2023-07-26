using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webmvc.Services;
using Microsoft.Extensions.Logging;

namespace webmvc.Controllers
{
    public class PlanetController : Controller
    {
        private readonly  PlanetServices _planetServices;
         private readonly ILogger<FirstController> _logger;

        public PlanetController(ILogger<FirstController> logger,    PlanetServices planets){
            _logger = logger;
            _planetServices = planets ;
        }

        [Route("danhsachcachanhtinh.html")]
        public IActionResult Index()
        {
            return View();
        }
        
        // route : action
        [BindProperty(SupportsGet = true, Name = "action")]
        public string Names{get;set;}
        public IActionResult Mercury() // khi truy cap action Mercury se co Name = Mercury, ten action == ten PlanetModel
        {
           var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Venus()
        {
             var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Earth()
        {
             var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Mars()
        {
            var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        [HttpGet("/saomoc.html")]
        public IActionResult Jupiter()
        {
             var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Saturn()
        {
             var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Uranus()
        {
            var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
        
        public IActionResult Neptune()
        {
            var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }
         public IActionResult Comet()
        {
             var planet = _planetServices.Where(p =>p.Name == Names).FirstOrDefault();
            return View("Detail", planet);
        }

    }
}