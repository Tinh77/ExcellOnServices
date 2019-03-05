using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Employee.Controllers
{
    public class LoginController : Controller
    {
        public static Models.Employee CurEmployee = new Models.Employee();

        ExcellOnServicesContext db = new ExcellOnServicesContext();
        // GET: Employee/Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login([FromUri] string email, [FromUri] string password)
        {
            if (Session["EmployeeLogin"] == null)
            {
                Models.Employee employee = db.Employees.Where(x => x.Email == email).FirstOrDefault();
                if (employee == null)
                {
                    return View("Index");
                }
                else
                {
                    if (password.CompareTo(employee.Password) == 0)
                    {
                        Session["EmployeeLogin"] = employee;
                        CurEmployee = employee;
                        return RedirectToAction("Index", "Dashboard", new { area = "Employee" });
                    }
                    else
                    {
                        return View("Index");
                    }
                }

            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return View("Index");
        }
    }
}