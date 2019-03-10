using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class PageNotFoundController : Controller
    {
        // GET: Customer/PageNotFound
        public ActionResult Index()
        {
            return View();
        }
    }
}