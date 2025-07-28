using D_Chandrakant.Models;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Security;
using Microsoft.AspNetCore.Http;
using D_Chandrakant.DataModels;

namespace D_Chandrakant.Controllers
{
    public class AuthAdController : Controller
    {
        private readonly TailordbContext _tailordbContext;

        public AuthAdController(ILogger<AuthAdController> logger, TailordbContext tailordbContext)
        {
            _tailordbContext = tailordbContext;
        }

        public IActionResult AdminLogin()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(AuthLoginModel model)
        {
            if(ModelState.IsValid ) {
                var Admin = _tailordbContext.Employees.FirstOrDefault(x => x.UserName==model.Username && x.Password==model.Password);

                if(Admin !=null )
                {
                    //HttpContext.Session.SetString("Name",Admin.ToString());

                    return RedirectToAction("EmployeeManagement", "Admin");

                }
                else
                {
                    ViewData["ErrorMessage"] = "Invalid username or password.";

                }


            }

            return View(model);

        }

    }
}
