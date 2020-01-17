using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Alithea2.Models;
using LinqKit;

namespace Alithea2.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db = new MyDbContext();

        public ActionResult Index()
        {
            List<ProductCategory> listProNam = new List<ProductCategory>();
            List<ProductCategory> listProNu = new List<ProductCategory>();
            List<ProductCategory> listGiayNam = new List<ProductCategory>();
            List<ProductCategory> listGiayNu = new List<ProductCategory>();

            List<Category> listCategories = new List<Category>();
            try
            {
                listProNam = db.ProductCategories.Where(pc => pc.CategoryID == 1).Take(4).ToList();
                listProNu = db.ProductCategories.Where(pc => pc.CategoryID == 2).Take(4).ToList();
                listGiayNam = db.ProductCategories.Where(pc => pc.CategoryID == 4).Take(4).ToList();
                listGiayNu = db.ProductCategories.Where(pc => pc.CategoryID == 5).Take(4).ToList();
                listCategories = db.Categories.Take(3).ToList();

                ViewBag.listProNam = listProNam;
                ViewBag.listProNu = listProNu;
                ViewBag.listGiayNam = listGiayNam;
                ViewBag.listGiayNu = listGiayNu;
                ViewBag.listCategories = listCategories;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }

        public ActionResult Search(string productname)
        {
            ViewBag.productname = productname;

            if (productname == null)
            {
                return Redirect("/Home");
            }

            try
            {
                ViewBag.searchProduct = db.Products.Where(p => p.ProductName.Contains(productname)).ToList();
                ViewBag.listCategories = db.Categories.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return View("Filter");
        }

        public ActionResult Filter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ViewBag.categoryFilter = db.Categories.Where(c => c.CategoryID == id).ToList();
                ViewBag.listCategories = db.Categories.ToList();
                ViewBag.listProductCategories = db.ProductCategories.Where(pc => pc.CategoryID == id).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Filter(List<int> idCat, int? price)
        {
            var categoryFilter = new List<Category>();
            var listProductCategories = db.ProductCategories.ToList();

            if (price != null)
            {
                if (price == 1)
                {
                    listProductCategories = listProductCategories.Where(pc => pc.Product.UnitPrice <= 150000).ToList();
                    ViewBag.PriceFilter = "<= 150.000";
                }else if (price == 2)
                {
                    listProductCategories = listProductCategories.Where(pc => pc.Product.UnitPrice >= 150000 && pc.Product.UnitPrice <= 500000).ToList();
                    ViewBag.PriceFilter = "150.000 - 500.000";
                }
                else if (price == 3)
                {
                    listProductCategories = listProductCategories.Where(pc => pc.Product.UnitPrice >= 500000 && pc.Product.UnitPrice <= 1000000).ToList();
                    ViewBag.PriceFilter = "500.000 - 1.000.000";
                }
                else if (price == 4)
                {
                    listProductCategories = listProductCategories.Where(pc => pc.Product.UnitPrice > 1000000).ToList();
                    ViewBag.PriceFilter = ">= 1.000.000";
                }

            }

            if (idCat != null)
            {
                var predicate = PredicateBuilder.New<ProductCategory>();

                try
                {
                    for (int i = 0; i < idCat.Count; i++)
                    {
                        Debug.WriteLine("catid: " + idCat[i]);
                        int CatId = idCat[i];
                        predicate = predicate.Or(pc => pc.CategoryID == CatId);
                        categoryFilter.Add(db.Categories.Find(CatId));
                    }

                    listProductCategories = listProductCategories.Where(predicate.Compile()).Select(pc => pc)
                        .GroupBy(pc => pc.ProductID).Where(pc => pc.Count() >= idCat.Count).Select(pc => pc.FirstOrDefault()).ToList();

                    for (int i = 0; i < listProductCategories.Count; i++)
                    {
                        Debug.WriteLine("name: " + listProductCategories[i].Product.ProductName);
                    }

                    ViewBag.categoryFilter = categoryFilter;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                listProductCategories = listProductCategories.Select(pc => pc).GroupBy(pc => pc.ProductID).Select(pc => pc.FirstOrDefault()).ToList();
            }

            ViewBag.listProductCategories = listProductCategories;
            ViewBag.listCategories = db.Categories.ToList();

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //------------------------
            Product product = new Product();
            List<ProductCategory> listProductCategories = new List<ProductCategory>();
            try
            {
                product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                listProductCategories = db.ProductCategories.Where(pc => pc.ProductID == id).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("error: " + e.Message);
            }
            ViewBag.listProductCategories = listProductCategories;
            return View(product);
        }

        public ActionResult Login()
        {
            if (CheckUser())
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,Password")] UserAccount userAccount)
        {
            Dictionary<String, String> errors = userAccount.ValidateLogin();
            if (errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View();
            }

            userAccount.Password = HasPass(userAccount.Password);
            UserAccount getUserAccount = null;
            try
            {
                getUserAccount = db.UserAccounts.FirstOrDefault(u => u.Username == userAccount.Username && u.Password == userAccount.Password);
            }
            catch (Exception e)
            {
                Debug.WriteLine("loi: " + e.Message);
            }


            if (getUserAccount != null)
            {
                Session["UserAccount"] = getUserAccount;
                return Redirect("/Home/Index");
            }
            else
            {
                errors.Add("ErrorLogin", "Tài khoản hoặc mật khẩu không đúng");
            }

            ViewBag.Errors = errors;
            return View();
        }

        public ActionResult Register()
        {
            if (CheckUser())
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount userAccount, string confirmpassord)
        {
            Dictionary<String, String> errors = userAccount.ValidateRegister();
            if (confirmpassord == "")
            {
                errors.Add("ConfirmPassword", "Bạn chưa nhập lại mật khẩu");
            }
            else if (userAccount.Password.Equals(confirmpassord) == false)
            {
                errors.Add("ConfirmPassword", "Mật khẩu nhập lại không đúng");
            }


            if (!errors.ContainsKey("Username") && !errors.ContainsKey("Email"))
            {
                //kiểm tra xem đã tồn tại username và email chưa
                Debug.WriteLine("dang chay database");
                List<UserAccount> getListUserAccount = new List<UserAccount>();
                try
                {
                    getListUserAccount = db.UserAccounts.Where(u => u.Username == userAccount.Username || u.Email == userAccount.Email).ToList();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                if (getListUserAccount != null && getListUserAccount.Count > 0)
                {
                    Debug.WriteLine("count: " + getListUserAccount.Count);
                    for (int i = 0; i < getListUserAccount.Count; i++)
                    {
                        if (userAccount.Username.Equals(getListUserAccount[i].Username))
                        {
                            errors["Username"] = "Tài khoản này đã tồn tại";
                        }
                        if (userAccount.Email.Equals(getListUserAccount[i].Email))
                        {
                            errors["Email"] = "Email này đã tồn tại";
                        }
                    }
                }
            }

            //tất cả đều hợp lệ
            if (errors.Count == 0)
            {
                
                int size = 10;
                string rollnumber = "123";
                UserAccount getUserAccount = null;
                do
                {
                    rollnumber = RandomString(size);
                    Debug.WriteLine("new roll: " + rollnumber);
                    try
                    {
                        getUserAccount = getUserAccount = db.UserAccounts.FirstOrDefault(u => u.RoleNumber == rollnumber);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return Redirect("/Home");
                    }
                    
                    size++;
                } while (getUserAccount != null);

                try
                {
                    userAccount.RoleNumber = rollnumber;
                    userAccount.admin = UserAccount.Decentralization.Customer;
                    userAccount.Password = HasPass(userAccount.Password);
                    userAccount.CreatAt = DateTime.Now;
                    userAccount.UpdateAt = DateTime.Now;
                    userAccount.Status = UserAccount.UserAccountStatus.Active;
                    userAccount.Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRQ8xzdv564ewROcTBYDdv51oTD5SgNOCDDwMw4XXIdvxFGyQzn&s";
                    db.UserAccounts.Add(userAccount);
                    db.SaveChanges();

                    return Redirect("/Home/Login");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("loi dang ky" + e.Message);
                    errors.Add("ErrorRegister", "Đăng ký thất bại");
                }
            }

            ViewBag.Errors = errors;
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

        private string HasPass(string pass)
        {
            //Tạo MD5 
            MD5 mh = MD5.Create();
            //Chuyển kiểu chuổi thành kiểu byte
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pass);
            //mã hóa chuỗi đã chuyển
            byte[] hash = mh.ComputeHash(inputBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public ActionResult ListCategory()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult DeleteSessionUser()
        {
            Session["UserAccount"] = null;
            Session["Customer"] = null;
            return Redirect("/Home/Index");
        }

        public bool CheckUser()
        {
            UserAccount userAccount = Session["UserAccount"] as UserAccount;
            if (userAccount != null)
            {
                return true;
            }

            return false;
        }
    }
}