using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Alithea2.Controllers.Service.ShopManager;
using Alithea2.Models;
using Newtonsoft.Json;

namespace Alithea2.Controllers
{

    public class BuyItemController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private ShopService _shopService = new ShopService();

        BuyItem buyItem = new BuyItem();
        // GET: BuyItem
        public ActionResult Index()
        {
            Debug.WriteLine("dang chay Index");
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.Sizes = db.Sizes.ToList();

            return View();
        }

        public int AddItem(int pro_id, int quantity, int color, string nameColor, int size, string nameSize)
        {
            Debug.WriteLine("dang chay add item");

            var hashtable = _shopService.AddItem(Session["buyItem"] as List<Product>, pro_id, quantity, color, nameColor, size, nameSize);

            Session["buyItem"] = hashtable["listShoppingCart"];
            Session["TotalPrice"] = (double)hashtable["TotalPrice"];
            Session["TotalQuantity"] = (int)hashtable["TotalQuantity"];

            if (Session["TotalQuantity"] != null)
            {
                return (int)Session["TotalQuantity"];
            }
            return quantity;
        }

        [HttpPost]
        public string DeleteItem(int idItem)
        {
            var hashtable = _shopService.DeleteItem(Session["buyItem"] as List<Product>, idItem);

            Session["buyItem"] = hashtable["listShoppingCart"];
            Session["TotalPrice"] = (double) hashtable["TotalPrice"];
            Session["TotalQuantity"] = (int) hashtable["TotalQuantity"];

            return JsonConvert.SerializeObject(hashtable);
        }

        [HttpPost]
        public string UpdateItem(int idItem, int quantity)
        {
            var hashtable = _shopService.UpdateItem(Session["buyItem"] as List<Product>, idItem, quantity);

            Session["buyItem"] = hashtable["listShoppingCart"];
            Session["TotalPrice"] = (double)hashtable["TotalPrice"];
            Session["TotalQuantity"] = (int)hashtable["TotalQuantity"];

            return JsonConvert.SerializeObject(hashtable);
        }

        public ActionResult ThongTinKhacHang()
        {
            if (Session["buyItem"] == null)
            {
                return RedirectToAction("Index");
            }

            Customer customer = new Customer();
            if (Session["UserAccount"] != null)
            {
                try
                {
                    UserAccount userAccount = Session["UserAccount"] as UserAccount;
                    customer.FullName = userAccount.FullName;
                    customer.Email = userAccount.Email;
                    customer.Address = userAccount.Address;
                    customer.Phone = userAccount.Phone;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else if(Session["Customer"] != null)
            {
                customer = Session["Customer"] as Customer;
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinKhacHang([Bind(Include = "ID,FullName,Address,Phone,Email")] Customer customer)
        {
            customer.Display();
            Dictionary<string, string> errors = customer.ValidateRegister();

            if (errors.Count == 0)
            {
                Session["Customer"] = customer;
                return RedirectToAction("XacNhanDonHang");
            }

            ViewBag.errors = errors;
            return View(customer);
        }

        public ActionResult XacNhanDonHang()
        {
            if (Session["buyItem"] == null)
            {
                Debug.WriteLine("ko co");
                return Redirect("/BuyItem/Index");
            }

            if (Session["Customer"] == null)
            {
                TempData["Error"] = "Bạn phải đăng nhập";
                return Redirect("/BuyItem/Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult XacNhanDonHang(string comment)
        {
            Debug.WriteLine("--------------");
            Debug.WriteLine("Dang chay xac nhan don hang");
            //Kiểm tra danh sách sản phẩm và thông tin đăng nhập
            if (Session["buyItem"] == null || Session["Customer"] == null)
            {
                return Redirect("/BuyItem/Index");
            }

            var createAt = DateTime.Now;

            if (_shopService.createOrder(Session["buyItem"] as List<Product>, createAt, Session["Customer"] as Customer,
                (int) Session["TotalQuantity"], (double) Session["TotalPrice"], comment,
                (Session["UserAccount"] as UserAccount)?.UserID))
            {
                var result = SendEmail(createAt.ToFileTimeUtc().ToString(), (Session["Customer"] as Customer)?.Email);
                Debug.WriteLine("result: " + result);
                
                DeleteSession();
                TempData["Success"] = "Đặt hàng thành công";
                TempData["RollNumber"] = createAt.ToFileTimeUtc().ToString();
                TempData["Email"] = (Session["Customer"] as Customer)?.Email;

                return Redirect("/BuyItem/Success");
            }

            TempData["Error"] = "Đã xảy ra lỗi";
            return View();
        }

        private string RandomString(int size)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                int a = rand.Next(1, 4);
                if (a == 1)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(48, 57)));
                }
                else if (a == 2)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 90)));
                }
                else
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(97, 122)));
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        public void DeleteSession()
        {
            Session["buyItem"] = null;
            Session["TotalPrice"] = null;
            Session["TotalQuantity"] = null;
        }

        public void test()
        {
            Order getOrder = db.Orders.FirstOrDefault(o => o.RoleNumber == "1rAvttt334");
            getOrder.Display();
        }

        public ActionResult Success()
        {
            if (TempData["Success"] == null || TempData["Success"] == null)
            {
                return Redirect("/BuyItem/Index");
            }
            return View();
        }

        public async Task<JsonResult> SendEmail(string rollnumber, string _email)
        {
            string senderID = "bangnguyenzero@gmail.com";
            string senderPassword = "Bang@123";
            string result = "Email Sent Successfully";

            string body = "Đơn hàng #" + rollnumber + " đã sẵn sàng giao đến quý khách";
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_email);
                mail.From = new MailAddress(senderID);
                mail.Subject = "Đơn đặt hàng Alithea";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                result = "problem occurred";
                Response.Write("Exception in sendEmail:" + ex.Message);
            }
            return Json(result);
        }

        public class TotalShoppingCart
        {
            public int TotalQuantity;
            public double TotalPrice;

        }
    }
}