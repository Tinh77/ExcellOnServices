using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class DashboardController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Customer/Dashboard
        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }
    }
}