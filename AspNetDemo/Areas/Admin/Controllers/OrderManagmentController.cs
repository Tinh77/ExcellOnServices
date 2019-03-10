using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;
using PagedList;
using PagedList.Mvc;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class OrderManagmentController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Admin/OrderManagment
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            return View(db.OrderServices.OrderBy(x => x.Status == 0).OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            var sTuKhoa = f["txtTimKiem"].ToString();
            List<OrderService> listKQTK = db.OrderServices.Where(n => n.Company.Name.Contains(sTuKhoa)).Where(n=>n.Company.Email.Contains(sTuKhoa)).Where(n=>n.Company.Id.Equals(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.OrderServices.OrderBy(n => n.Company.Name).ToPagedList(pageNumber, pageSize));
            }
            return View(listKQTK.OrderBy(n => n.Company.Name).ToPagedList(pageNumber, pageSize));
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

        public ActionResult Cancel(int id)
        {
            OrderService order = new OrderService();
            order = db.OrderServices.Find(id);
            if (order.Status == 0)
            {
                order.Status = -1;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult ReOpen(int id)
        {
            OrderService order = new OrderService();
            order = db.OrderServices.Find(id);
            if (order.Status == -1)
            {
                order.Status = 0;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }


        public ActionResult ChangeStatus(int id)
        {
            OrderService order = new OrderService();
            order = db.OrderServices.Find(id);
            if (order.Status == 0)
            {
                order.Status = 1;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (order.Status == 1)
            {
                order.Status = 2;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //else if (order.Status == 2)
            //{
            //    order.Status = 3;
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View("Index");
        }

    }
}