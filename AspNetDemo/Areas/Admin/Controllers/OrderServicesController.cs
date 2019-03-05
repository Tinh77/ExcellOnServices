using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class OrderServicesController : Controller
    {
        private ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Admin/OrderServices
        public ActionResult Index()
        {
            var orderServices = db.OrderServices.Include(o => o.Company);
            return View(orderServices.ToList());
        }

        // GET: Admin/OrderServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderService orderService = db.OrderServices.Find(id);
            if (orderService == null)
            {
                return HttpNotFound();
            }
            return View(orderService);
        }

        // GET: Admin/OrderServices/Create
        public ActionResult Create()
        {
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name");
            return View();
        }

        // POST: Admin/OrderServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,TotalPrice,CreatedAt,PaymentDate,BillDate,Status,Company_Id")] OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                db.OrderServices.Add(orderService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", orderService.Company_Id);
            return View(orderService);
        }

        // GET: Admin/OrderServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderService orderService = db.OrderServices.Find(id);
            if (orderService == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", orderService.Company_Id);
            return View(orderService);
        }

        // POST: Admin/OrderServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,TotalPrice,CreatedAt,PaymentDate,BillDate,Status,Company_Id")] OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", orderService.Company_Id);
            return View(orderService);
        }

        // GET: Admin/OrderServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderService orderService = db.OrderServices.Find(id);
            if (orderService == null)
            {
                return HttpNotFound();
            }
            return View(orderService);
        }

        // POST: Admin/OrderServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderService orderService = db.OrderServices.Find(id);
            db.OrderServices.Remove(orderService);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
