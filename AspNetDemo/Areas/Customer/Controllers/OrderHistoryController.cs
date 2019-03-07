using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class OrderHistoryController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();
        // GET: Customer/OrderHistory
        public ActionResult Index()
        {
            Company curCompany = (Company)Session["CompanyLogin"];
            var listOrd = db.OrderServices.Where(x => x.Company_Id== curCompany.Id).OrderByDescending(x => x.CreatedAt).ToList();
            return View(listOrd);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderService order = db.OrderServices.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

    }
}