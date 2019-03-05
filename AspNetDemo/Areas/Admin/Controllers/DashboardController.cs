using AspNetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();
        // GET: Admin/Dashboard
        public ActionResult ListCompany()
        {
            return View(db.Companies.ToList());
        }

        public ActionResult ListEmployee()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult ListEmployeeActive()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult ListEmployeeDeactive()
        {
            return View(db.Employees.ToList());
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
            return View(db.OrderServices.ToList());
        }

        public ActionResult ListOrder_Deactive()
        {
            return View(db.OrderServices.ToList());
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderService order = db.OrderServices.Find(id);
            if(order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderService order = db.OrderServices.Find(id);
            db.OrderServices.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}