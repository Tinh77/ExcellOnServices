using System;
using System.Collections.Generic;
using System.Data;
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
    public class EmployeesController : Controller
    {
        private ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Admin/Employees
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var employees = db.Employees.OrderBy(e => e.Username).ToPagedList(pageNumber,pageSize);
            return View(employees);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            if (Session["CompanyLogin"] == null)
            {

            }
                var sTuKhoa = f["txtTimKiem"].ToString();
            List<Models.Employee> listKQTK = db.Employees.Where(n => n.Username.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Employees.OrderBy(n => n.Username).ToPagedList(pageNumber, pageSize));
            }


            return View(listKQTK.OrderBy(n => n.Username).ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Admin/Employees/Create
        public ActionResult Create()
        {
            ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name");
            return View();
        }

        // POST: Admin/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Gender,PhoneNumber,Email,Address,Avatar,Department_Id,Status,Username,Password")] Models.Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", employee.Department_Id);
            return View(employee);
        }

        // GET: Admin/Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", employee.Department_Id);
            return View(employee);
        }

        // POST: Admin/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Gender,PhoneNumber,Email,Address,Avatar,Department_Id,Status,Username,Password")] Models.Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", employee.Department_Id);
            return View(employee);
        }

        // GET: Admin/Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Admin/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetDemo.Models.Employee cat = new AspNetDemo.Models.Employee();
            AspNetDemo.Models.Admin adm = (AspNetDemo.Models.Admin)Session["Admin"];
            cat = db.Employees.Find(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cat.Status == 1)
                {
                   
                    cat.Status = -1;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Index");

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
