using D_Chandrakant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using D_Chandrakant.DataModels;



namespace D_Chandrakant.Controllers
{
    public class AuthEmpController : Controller
    {
        private readonly TailordbContext _tailordbContext;

        public AuthEmpController(ILogger<AuthEmpController> logger, TailordbContext tailordbContext)
        {
            _tailordbContext = tailordbContext;
        }

         public IActionResult EmpLogin()
         {
            if (HttpContext.Session.GetInt32("ID") != null)
            {

                return RedirectToAction("EmpPage", "Emp");
            }

            return View();
         }

        [HttpPost]

         public IActionResult EmpLogin(AuthEmpModel model)
         {
             if (ModelState.IsValid)
             {
                 var Employee=_tailordbContext.Emps.FirstOrDefault(x=>x.Id == model.Id && x.RecStatus=="A");

                 if (Employee!=null) 
                 {

                    HttpContext.Session.SetInt32("ID", Employee.Id);
                    HttpContext.Session.SetString("IDNew", "5");

                    return RedirectToAction("EmpPage", "Emp");

                 }
                 else
                 {
                     ViewData["ErrorMessage"] = "Please Enter Correct ID";
                    
                 }
             }

             return View(model);


         }

        public IActionResult EmpLogout()
        {

            if (HttpContext.Session.GetInt32("ID") != null)
            {

                HttpContext.Session.Remove("ID");
                return RedirectToAction("EmpLogin");
            }


            return View();

        }






    }
}
