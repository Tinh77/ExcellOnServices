using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: Admin/LoginAdmin
        public static Models.Admin CurAdmin = new Models.Admin();

        ExcellOnServicesContext db = new ExcellOnServicesContext();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login([FromUri] string email, [FromUri] string password)
        {
            if (Session["AdminLogin"] == null)
            {
                Models.Admin admin = db.Admins.Where(x => x.email == email).FirstOrDefault();
                if (admin == null)
                {
                    return View("Index");
                }
                else
                {
                    if (password.CompareTo(admin.password) == 0)
                    {
                        Session["AdminLogin"] = admin;
                        CurAdmin = admin;
                        return RedirectToAction("Index", "Companies", new { area = "Admin" });
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