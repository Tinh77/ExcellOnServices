using AspNetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();
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

        public ActionResult ListServices()
        {
            return View(db.Services.ToList());
        }

        public ActionResult ListOrder()
        {
            return View(db.OrderServices.ToList());
        }

        public ActionResult ListOrder_Active()
        {
            return View();
        }

        public ActionResult ListOrder_Deactive()
        {
            return View();
        }


    }
}