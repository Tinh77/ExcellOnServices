using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        public static Company CurCompany = new Company();

        ExcellOnServicesContext db = new ExcellOnServicesContext();
        // GET: Customer/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login([FromUri] string email, [FromUri] string password)
        {

            if (Session["CompanyLogin"] == null)
            {
                Company company = db.Companies.Where(x => x.Email == email).FirstOrDefault();
                if (company == null)
                {
                    return View("Index");
                }
                else
                {
                    if (password.CompareTo(company.Password) == 0)
                    {
                        Session["CompanyLogin"] = company;
                        CurCompany = company;
                        return Redirect("/Home/Index");
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