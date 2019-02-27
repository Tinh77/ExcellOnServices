using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using AspNetDemo.Models;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class RegisterController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Customer/Register
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            ViewBag.Dropdown = (from c in db.Payment_Method select new { c.id, c.Pay_Name }).Distinct();
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(Company company)
        {
            if (ModelState.IsValid)
            {
                var isExistEmail = IsEmailExist(company.Email);
                if (isExistEmail)
                {
                    ModelState.AddModelError(company.Email, "Email already exist");
                    return View(company);
                }

                company.Payment_Method_id = 1;
                company.Status = 1;

                db.Companies.Add(company);
                db.SaveChanges();
            }
            return View(company);
        }

        [System.Web.Mvc.NonAction]
        public bool IsEmailExist(string email)
        {
            using (ExcellOnServicesContext db = new ExcellOnServicesContext())
            {
                var v = db.Companies.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }

    }
}