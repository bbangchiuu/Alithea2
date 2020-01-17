using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Alithea2.Models;

namespace Alithea2.Controllers
{
    public class BuyItemController : Controller
    {
        private MyDbContext db = new MyDbContext();
        BuyItem buyItem = new BuyItem();
        // GET: BuyItem
        public ActionResult Index()
        {
            Debug.WriteLine("dang chay Index");
            listproductorderdetail();
            return View();
        }

        public int AddItem(int pro_id, int quantity, int color, int size)
        {
            Debug.WriteLine("sizessss: " + size);
            listproductorderdetail();
            Console.WriteLine("dang chay add item");
            if (Session["buyItem"] == null)
            {

                Product product = new Product();
                try
                {
                    product = db.Products.Find(pro_id);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                if (product == null)
                {
                    return 0;
                }

                buyItem.AddProduct(product, quantity, color, size);
                Session["TotalQuantity"] = quantity;
                Session["TotalPrice"] = product.UnitPrice * quantity;
                Session["buyItem"] = buyItem.ListProducts;
            }
            else
            {
                buyItem.ListProducts = Session["buyItem"] as List<Product>;
                Boolean checkSP = true;
                for (int i = 0; i < buyItem.ListProducts.Count; i++)
                {
                    buyItem.ListProducts[i].Display();
                    if (buyItem.ListProducts[i].ProductID == pro_id 
                        && buyItem.ListProducts[i].Color == (Product.ColorProduct?) color 
                        && buyItem.ListProducts[i].Size == (Product.SizeProduct?) size)
                    {
                        buyItem.ListProducts[i].Quantity += quantity;
                        checkSP = false;
                        break;
                    }
                }
                if (checkSP)
                {
                    Product product = db.Products.Find(pro_id);
                    buyItem.AddProduct(product, quantity, color, size);
                }

                ListOrder(buyItem.ListProducts);
            }

            if (Session["TotalQuantity"] != null)
            {
                return (int)Session["TotalQuantity"];
            }
            return quantity;
        }

        public ActionResult UpdateQuantity(int pro_id, int quantity, int? color, int? size)
        {

            if (color == null)
            {
                color = 0;
            }

            if (size == null)
            {
                size = 0;
            }

            Debug.WriteLine("color: " + color);
            Debug.WriteLine("size: " + size);

            if (Session["buyItem"] != null)
            {
                buyItem.ListProducts = Session["buyItem"] as List<Product>;
                for (int i = 0; i < buyItem.ListProducts.Count; i++)
                {
                    buyItem.ListProducts[i].Display();
                    if (buyItem.ListProducts[i].ProductID == pro_id
                        && buyItem.ListProducts[i].Color == (Product.ColorProduct?)color
                        && buyItem.ListProducts[i].Size == (Product.SizeProduct?)size)
                    {
                        buyItem.ListProducts[i].Quantity += quantity;
                        if (buyItem.ListProducts[i].Quantity <= 0)
                        {
                            buyItem.ListProducts.RemoveAt(i);
                        }
                        break;
                    }
                }

                ListOrder(buyItem.ListProducts);
            }

            return Redirect("/BuyItem/Index");
        }

        public ActionResult Delete_order(int pro_id, int? color, int? size)
        {
            buyItem.ListProducts = Session["buyItem"] as List<Product>;

            for (int i = 0; i < buyItem.ListProducts.Count; i++)
            {
                if (buyItem.ListProducts[i].ProductID == pro_id
                    && buyItem.ListProducts[i].Color == (Product.ColorProduct?)color
                    && buyItem.ListProducts[i].Size == (Product.SizeProduct?)size)
                {
                    Console.WriteLine("id: " + buyItem.ListProducts.ElementAt(i).ProductID);
                    buyItem.ListProducts.RemoveAt(i);
                }
            }

            if (buyItem.ListProducts.Count == 0)
            {
                Session["TotalQuantity"] = null;
                Session["TotalPrice"] = null;
                Session["buyItem"] = null;
            }
            else
            {
                ListOrder(buyItem.ListProducts);
            }

            return Redirect("/BuyItem/Index");
        }

        public void ListOrder(List<Product> list_pro)
        {
            buyItem.ListProducts = list_pro;

            if (buyItem.ListProducts.Count == 0)
            {
                Session["buyItem"] = null;
            }
            else
            {
                Session["buyItem"] = buyItem.ListProducts;
            }

            double totalprice = 0;
            var totalQuantity = 0;
            foreach (var val in buyItem.ListProducts)
            {
                totalprice = (double)(totalprice + val.UnitPrice * val.Quantity);
                totalQuantity = (int)(totalQuantity + val.Quantity);
            }

            Session["TotalQuantity"] = totalQuantity;
            Session["TotalPrice"] = totalprice;
        }

        public void listproductorderdetail()
        {
            List<Product> listProducts = new List<Product>();
            if (Session["buyItem"] != null)
            {
                listProducts = Session["buyItem"] as List<Product>;
            }
            Debug.WriteLine("count: " + listProducts.Count);
            Debug.WriteLine("----------------");
            for (int i = 0; i < listProducts.Count; i++)
            {
                listProducts[i].Display();
                Debug.WriteLine("----------------");
            }

            int TotalQuantity = 0;
            if (Session["TotalQuantity"] != null)
            {
                TotalQuantity = (int)Session["TotalQuantity"];
            }

            double TotalPrice = 0;
            if (Session["TotalPrice"] != null)
            {
                TotalPrice = (double)Session["TotalPrice"];
            }

            Debug.WriteLine("total quanitty " + TotalQuantity);
            Debug.WriteLine("total price " + TotalPrice);

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

            try
            {
                Customer customer = Session["Customer"] as Customer;
                if (customer == null)
                {
                    return View();
                }

                DateTime createAt = DateTime.Now;
                customer.Display();
                Debug.WriteLine("total quanitty " + Session["TotalQuantity"]);
                Debug.WriteLine("total price " + Session["TotalPrice"]);

                //Add Order
                int TotalQuantity = 0;
                if (Session["TotalQuantity"] != null)
                {
                    TotalQuantity = (int)Session["TotalQuantity"];
                }

                double TotalPrice = 0;
                if (Session["TotalPrice"] != null)
                {
                    TotalPrice = (double)Session["TotalPrice"];
                }

                int size = 10;
                string rollnumber = "123";
                Order getOrder = null;
                do
                {
                    rollnumber = RandomString(size);
                    Debug.WriteLine("new roll: " + rollnumber);
                    try
                    {
                        getOrder = getOrder = db.Orders.FirstOrDefault(u => u.RoleNumber == rollnumber);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return Redirect("/Home");
                    }

                    size++;
                } while (getOrder != null);

                int? userID = null;
                if (Session["UserAccount"] != null)
                {
                    userID = (Session["UserAccount"] as UserAccount).UserID;
                }

                Order order = new Order()
                {
                    RoleNumber = rollnumber,
                    OrderDate = createAt,
                    RequireDate = null,
                    ShippedDate = null,
                    Quantity = 0,
                    TotalPrice = 0,
                    Status = Order.StatusOrder.DeActive,
                    UserID = userID,
                    Commnet = comment,
                    FullName =  customer.FullName,
                    Email = customer.Email,
                    Address = customer.Address,
                    Phone = customer.Phone
                };
                db.Orders.Add(order);
                order.Display();
                db.SaveChanges();

                //get OrderId
                getOrder = db.Orders.FirstOrDefault(o => o.RoleNumber == rollnumber);
                getOrder.Display();

                var listOrderDetail = new List<OrderDetail>();
                buyItem.ListProducts = Session["buyItem"] as List<Product>;

                Debug.WriteLine("count: " + buyItem.ListProducts.Count);
                //Add OrderDetail
                for (int i = 0; i < buyItem.ListProducts.Count; i++)
                {
                    listOrderDetail.Add(new OrderDetail()
                    {
                        OrderID = getOrder.OrderID,
                        ProductID = buyItem.ListProducts[i].ProductID,
                        Quantity = buyItem.ListProducts[i].Quantity,
                        UnitPrice = buyItem.ListProducts[i].UnitPrice,
                        Color = (OrderDetail.ColorProduct?) buyItem.ListProducts[i].Color,
                        Size = (OrderDetail.SizeProduct?) buyItem.ListProducts[i].Size,
                    });
                }
                db.OrderDetails.AddRange(listOrderDetail);

                //update get Order
                getOrder.Quantity = TotalQuantity;
                getOrder.TotalPrice = TotalPrice;
                db.Entry(getOrder).State = EntityState.Modified;

                db.SaveChanges();

                var result = SendEmail(getOrder.RoleNumber, customer.Email);
                Debug.WriteLine("result: " + result);

                DeleteSession();
                TempData["Success"] = "Đặt hàng thành công";
                TempData["RollNumber"] = rollnumber;
                TempData["Email"] = getOrder.Email;
            }
            catch (Exception e)
            {
                Debug.WriteLine("loi roi: " + e.Message);
                TempData["Error"] = "Đã xảy ra lỗi";
                return View();
            }

            return Redirect("/BuyItem/Success");
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
    }
}