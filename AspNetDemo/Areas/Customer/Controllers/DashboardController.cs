using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;
using PagedList;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class DashboardController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Customer/Dashboard
        public ActionResult Index(int? page)
        {
            if (Session["CompanyLogin"] == null)
            {
                return RedirectToAction("Login", "Login", new { Area = "Customer" });
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;
            return View(db.OrderServices.OrderBy(x => x.Status == 2).OrderByDescending(x => x.CreatedAt).ToList());
        }
    }
}