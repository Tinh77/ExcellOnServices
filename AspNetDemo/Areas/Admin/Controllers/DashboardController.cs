﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        ExcellOnServicesContext db = new ExcellOnServicesContext();
        // GET: Admin/Dashboard
        public ActionResult ListCustomer()
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
    }
}