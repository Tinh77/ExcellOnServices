using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult ListCustomer()
        {
            return View();
        }
        public ActionResult ListEmployee()
        {
            return View();
        }
        public ActionResult ListEmployeeActive()
        {
            return View();
        }
        public ActionResult ListEmployeeDeactive()
        {
            return View();
        }
    }
}