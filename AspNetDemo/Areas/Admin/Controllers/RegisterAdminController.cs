using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class RegisterAdminController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Admin/Register
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            ViewBag.Dropdown = (from c in db.Payment_Method select new { c.id, c.Pay_Name }).Distinct();
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(Models.Admin admin)
        {
            if (ModelState.IsValid)
            {
                var isExistEmail = IsEmailExist(admin.email);
                if (isExistEmail)
                {
                    ModelState.AddModelError(admin.email, "Email already exist");
                    return View(admin);
                }
                db.Admins.Add(admin);
                db.SaveChanges();
            }
            return View(admin);
        }

        [System.Web.Mvc.NonAction]
        public bool IsEmailExist(string email)
        {
            using (ExcellOnServicesContext db = new ExcellOnServicesContext())
            {
                var v = db.Admins.Where(a => a.email == email).FirstOrDefault();
                return v != null;
            }
        }

    }
}