using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNetDemo.Models;

namespace AspNetDemo.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        private const string EMAIL = "excellonservice123@gmail.com";
        private const string PASSAPP = "kimlmjniyhctbguo";
        private const string SUBJECT = "Bạn đã đặt hàng thành công!";

        ExcellOnServicesContext db = new ExcellOnServicesContext();
        Company comp = LoginController.CurCompany;

        // GET: Customer/Cart
        public ActionResult Index()
        {
            return View();
        }

        public int isExisting([FromUri] int id)
        {
            List<OrderDetail> cart = (List<OrderDetail>) Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Service.Id == id)
                    return i;
            return -1;
        }

        public ActionResult Delete([FromUri] int id)
        {
            int index = isExisting(id);
            List<OrderDetail> cart = (List<OrderDetail>) Session["cart"];
            cart.RemoveAt(index);
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Home", new {area = ""});
            }
            else
            {
                Session["cart"] = cart;
                return View("Index");
            }
        }

        public ActionResult AddtoCart([FromUri] int id, [FromUri] string date, [FromUri] int numberOfEmployee)
        {
            string[] cutDate = date.Split('-');

            if (Session["cart"] == null)
            {
                List<OrderDetail> cart = new List<OrderDetail>();
                cart.Add(new OrderDetail(db.Services.Find(id), numberOfEmployee, Convert.ToDateTime(cutDate[0]), Convert.ToDateTime(cutDate[1])));
                Session["cart"] = cart;
            }
            else
            {
                List<OrderDetail> cart = (List<OrderDetail>) Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new OrderDetail(db.Services.Find(id), numberOfEmployee, Convert.ToDateTime(cutDate[0]), Convert.ToDateTime(cutDate[1])));
                else
                    cart[index].NumberOfEmployee++;
                Session["cart"] = cart;
            }

            return View("Index");
        }

        public ActionResult Checkout([FromUri] string totalPr, [FromUri] string description)
        {

            OrderService order = new OrderService();
            order.TotalPrice = totalPr;
            order.Description = description;
            order.CreatedAt = DateTime.Now;
            order.PaymentDate = DateTime.Now;
            order.BillDate = DateTime.Now;
            order.Company_Id = comp.Id;
            db.OrderServices.Add(order);
            db.SaveChanges();

            var ord = db.OrderServices.Where(x => x.Company_Id == comp.Id).ToList().OrderByDescending(i => i.Id)
                .FirstOrDefault();
            if (ord != null)
            {
                OrderDetail orderDetail = new OrderDetail();
                List<OrderDetail> cart = (List<OrderDetail>)Session["cart"];
                for (int i = 0; i < cart.Count; i++)
                {
                    orderDetail.OrderService_Id = ord.Id;
                    orderDetail.Service_Id = cart[i].Service.Id;
                    orderDetail.UnitPrice = Convert.ToInt32(cart[i].Service.Price);
                    orderDetail.NumberOfEmployee = cart[i].NumberOfEmployee;
                    orderDetail.FromDate = cart[i].FromDate;
                    orderDetail.ToDate = cart[i].ToDate;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                }
                Session.Remove("cart");
            }

            Company company = (Company) Session["CompanyLogin"];
            SendEmailToCompany(company.Email);
            return View("Success");
        }

        [System.Web.Mvc.NonAction]
        public void SendEmailToCompany(string email)
        {
            var fromEmail = new MailAddress(EMAIL, "Activation Username");
            var toEmail = new MailAddress(email);
            string subject = SUBJECT;
            var fromEmailPassword = PASSAPP;
            var ord = db.OrderServices.Where(x => x.Company_Id == comp.Id).ToList().OrderByDescending(i => i.Id)
                .FirstOrDefault();

            string body = "Cảm ơn bạn đã thuê dịch vụ của chúng tôi" +
                          "Mã đặt hàng của bạn là " + ord.Id;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

            smtp.Send(message);
        }
    }
}