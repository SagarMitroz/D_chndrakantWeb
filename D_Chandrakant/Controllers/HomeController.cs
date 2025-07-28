using D_Chandrakant.DataModels;
using D_Chandrakant.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace D_Chandrakant.Controllers
{
    public class HomeController : Controller
    {
        private readonly TailordbContext _tailordbContext;


        public HomeController(ILogger<HomeController> tailordbContext)
        {
            _tailordbContext = (TailordbContext?)tailordbContext;
        }


        public IActionResult Index()
        {
            
            return View();
        }

        

        /*public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}