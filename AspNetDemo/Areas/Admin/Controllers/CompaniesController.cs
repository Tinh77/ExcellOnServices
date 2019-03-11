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
    public class CompaniesController : Controller
    {
        private ExcellOnServicesContext db = new ExcellOnServicesContext();

        // GET: Admin/Companies
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["AdminLogin"] == null)
                {
                    return RedirectToAction("Login", "LoginAdmin", new { Area = "Admin" });
                }
                var pageNumber = page ?? 1;
                var pageSize = 5;
                var companies = db.Companies.OrderBy(c => c.Username).ToPagedList(pageNumber, pageSize);
                return View(companies);
            }
            catch (Exception)
            {
                return RedirectToAction("PageNotFound", "PageNotFound", new { Area = "Customer" });
                throw;
            }

        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            if (Session["AdminLogin"] == null)
            {
                return RedirectToAction("Login", "LoginAdmin", new { Area = "Admin" });
            }
            var sTuKhoa = f["txtTimKiem"].ToString();
            List<Company> listKQTK = db.Companies.Where(n => n.Name.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Companies.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
            }


            return View(listKQTK.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Companies/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (Session["AdminLogin"] == null)
                {
                    return RedirectToAction("Login", "LoginAdmin", new { Area = "Admin" });
                }
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Company company = db.Companies.Find(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
                return View(company);
            }
            catch (Exception)
            {
                return RedirectToAction("PageNotFound", "PageNotFound", new { Area = "Customer" });
                throw;
            }

        }

        // GET: Admin/Companies/Create
        public ActionResult Create()
        {
            if (Session["AdminLogin"] == null)
            {
                return RedirectToAction("Login", "LoginAdmin", new { Area = "Admin" });
            }
            ViewBag.Payment_Method_id = new SelectList(db.Payment_Method, "id", "Pay_Name");
            return View();
        }

        // POST: Admin/Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Logo,Phone,Email,Address,Status,Payment_Method_id,Username,Password")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Payment_Method_id = new SelectList(db.Payment_Method, "id", "Pay_Name", company.Payment_Method_id);
            return View(company);
        }

        // GET: Admin/Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["AdminLogin"] == null)
            {
                return RedirectToAction("Login", "LoginAdmin", new { Area = "Admin" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.Payment_Method_id = new SelectList(db.Payment_Method, "id", "Pay_Name", company.Payment_Method_id);
            return View(company);
        }

        // POST: Admin/Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Logo,Phone,Email,Address,Status,Payment_Method_id,Username,Password")] Company company)
        {

            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Payment_Method_id = new SelectList(db.Payment_Method, "id", "Pay_Name", company.Payment_Method_id);
            return View(company);
        }

        public ActionResult Delete(int id)
        {
            AspNetDemo.Models.Employee emp = new AspNetDemo.Models.Employee();
            AspNetDemo.Models.Admin adm = (AspNetDemo.Models.Admin)Session["Admin"];
            emp = db.Employees.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (emp.Status == 1)
                {
                    emp.Status = -1;
                    db.Entry(emp).State = EntityState.Modified;
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
