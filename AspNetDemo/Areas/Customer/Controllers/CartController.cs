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
            SendEmail(company.Email);
            return View("Success");
        }

        [System.Web.Mvc.NonAction]
        public void SendEmail(string email)
        {
            Company cus = (Company)Session["Customer"];
            var fromEmail = new MailAddress(EMAIL, "Excell On Service");
            var toEmail = new MailAddress(email);
            string subject = SUBJECT;
            var fromEmailPassword = PASSAPP;

            var ord = db.OrderServices.Where(x => x.Company_Id == cus.Id).ToList().OrderByDescending(i => i.Id)
                .FirstOrDefault();

            string itemText = null;

            var itemOrd = db.OrderDetails.Where(x => x.OrderService_Id == ord.Id).ToList();

            foreach (var item in itemOrd)
            {
                var service = db.Services.Where(x => x.Company_Id == item.id).FirstOrDefault();
                itemText += @"<tr class=""item"">
                <td>"
                           + service.Name + " " + "*" + item.NumberOfEmployee +
                           @"</td>
                
                <td>"
                           + item.NumberOfEmployee * Int32.Parse(item.Service.Price) +
                           @"</td>
            </tr>";
            }

            string body = @"<!doctype html><html><head><meta charset=""utf-8""><title>A simple, clean, and responsive HTML invoice template</title><style>.invoice-box {max-width: 800px;margin: auto;
        padding: 30px;
        border: 1px solid #eee;
        box-shadow: 0 0 10px rgba(0, 0, 0, .15);
        font-size: 16px;
        line-height: 24px;
        font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
        color: #555;
    }
    
    .invoice-box table {
        width: 100%;
        line-height: inherit;
        text-align: left;
    }
    
    .invoice-box table td {
        padding: 5px;
        vertical-align: top;
    }
    
    .invoice-box table tr td:nth-child(2) {
        text-align: right;
    }
    
    .invoice-box table tr.top table td {
        padding-bottom: 20px;
    }
    
    .invoice-box table tr.top table td.title {
        font-size: 45px;
        line-height: 45px;
        color: #333;
    }
    
    .invoice-box table tr.information table td {
        padding-bottom: 40px;
    }
    
    .invoice-box table tr.heading td {
        background: #eee;
        border-bottom: 1px solid #ddd;
        font-weight: bold;
    }
    
    .invoice-box table tr.details td {
        padding-bottom: 20px;
    }
    
    .invoice-box table tr.item td{
        border-bottom: 1px solid #eee;
    }
    
    .invoice-box table tr.item.last td {
        border-bottom: none;
    }
    
    .invoice-box table tr.total td:nth-child(2) {
        border-top: 2px solid #eee;
        font-weight: bold;
    }
    
    @media only screen and (max-width: 600px) {
        .invoice-box table tr.top table td {
            width: 100%;
            display: block;
            text-align: center;
        }
        
        .invoice-box table tr.information table td {
            width: 100%;
            display: block;
            text-align: center;
        }
    }
    
    /** RTL **/
    .rtl {
        direction: rtl;
        font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
    }
    
    .rtl table {
        text-align: right;
    }
    
    .rtl table tr td:nth-child(2) {
        text-align: left;
    }
    </style>
</head>

<body>
    <div class=""invoice-box"">
        <table cellpadding=""0"" cellspacing=""0"">
            <tr class=""top"">
                <td colspan=""2"">
                    <table>
                        <tr>
                            <td class=""title"">
                                <img src=""https://i.imgur.com/PxlYgOA.png"" style=""width:100%; max-width:300px;"">
                            </td>
                            
                            <td>
                                Invoice #:" + ord.Id + @"<br>" +
                                @"Created: " + DateTime.Now +
                                "<br> PaymentDate:" + " Ngày kết thúc" +
                            @"</td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr class=""information"">
                <td colspan=""2"">
                    <table>
                        <tr>
                            <td>
                                Yash, Inc.<br>
                                12345 Sunny Road<br>
                                Sunnyville, CA 12345
                            </td>
                            
                            <td>
                                Acme Corp.<br>" +
                                cus.Name + "<br>" +
                                cus.Email +
                           @"</td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr class=""heading"">
                <td>
                    Payment Method
                </td>
                
                <td>
                    Check #
                </td>
            </tr>
            
            <tr class=""details"">
                <td>
                    Check
                </td>
                
                <td>
                    1000
                </td>
            </tr>
            
            <tr class=""heading"">
                <td>
                    Item
                </td>
                
                <td>
                    Price
                </td>
            </tr>" + itemText +

            @"<tr class=""total"">
                <td></td> 
                <td>
                   Total:" + "$" + ord.TotalPrice +
               @"</td>
            </tr>
        </table>
    </div>
</body>
</html>";


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