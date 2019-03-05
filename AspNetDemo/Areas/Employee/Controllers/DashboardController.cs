using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetDemo.Areas.Employee.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Employee/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}