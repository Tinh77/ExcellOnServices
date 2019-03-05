using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Controllers
{
    public class HomeController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }

        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Service service = db.Services.Find(id);

            if(service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
    }
}