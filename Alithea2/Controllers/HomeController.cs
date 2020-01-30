using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Alithea2.Controllers.Service.AttributeManager;
using Alithea2.Controllers.Service.CategoryManager;
using Alithea2.Controllers.Service.ProductCateogryManager;
using Alithea2.Controllers.Service.ProductManager;
using Alithea2.Controllers.Service.ShopManager;
using Alithea2.Models;
using LinqKit;
using Attribute = System.Attribute;

namespace Alithea2.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private CategoryService _categoryService = new CategoryService();
        private ProductService _productService = new ProductService();
        private ShopService _shopService = new ShopService();
        private ProductCategoryService _productCategoryService = new ProductCategoryService();
        private AttributeService _attributeService = new AttributeService();

        public ActionResult Index()
        {
            try
            {
                ViewBag.listCategories = db.Categories.Take(3).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }

        public ActionResult Search(string productname, int? page, int? limit)
        {
            ViewBag.productname = productname;

            if (productname == null)
            {
                return Redirect("/Home");
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 6;
            }

            try
            {
                var productFilter = db.Products.Where(p => p.ProductName.Contains(productname)).ToList();

                ViewBag.TotalPage = Math.Ceiling((double)productFilter.Count() / limit.Value);
                ViewBag.CurrentPage = page;
                ViewBag.limit = limit;

                ViewBag.productFilter = productFilter.OrderByDescending(p => p.ProductName).Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
                ViewBag.listCategories = _categoryService.GetAll().ToList(); 
                ViewBag.currentPara = "?productname=" + productname + "&";

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return View("Filter");
        }

        public ActionResult Filter(List<int> id, int? page, int? limit, double? MinPrice, double? MaxPrice)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 6;
            }

            if (id == null && MinPrice == null && MaxPrice == null)
            {
                ViewBag.productFilter = _productService.listPagination(page, limit);
                ViewBag.TotalPage = Math.Ceiling((double)_productService.GetAll().Count() / limit.Value);
                ViewBag.currentPara = "?";
            }
            else
            {
                var hashtable = _shopService.FilterProduct(id, page.Value, limit.Value, MinPrice, MaxPrice);

                ViewBag.categoryFilter = hashtable["listCategory"];
                ViewBag.productFilter = hashtable["listProduct"];

                ViewBag.TotalPage = hashtable["totalPage"];
                ViewBag.currentPara = hashtable["currentPara"];
            }

            ViewBag.listCategories = _categoryService.GetAll().ToList();

            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;
          
            ViewBag.MinPrice = MinPrice;
            ViewBag.MaxPrice = MaxPrice;
         
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //------------------------
            var product = _productService.SelectById(id);
            ViewBag.listProductCategories = _productCategoryService.GetCategories(id);
            ViewBag.productAttribute = _attributeService.GetAttributesOfProduct(id);
            ViewBag.Size = db.Sizes.ToList();

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

        public ActionResult ListCategory(int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 9;
            }

            var listCategories = _categoryService.listPagination(page, limit);

            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;
            ViewBag.TotalPage = Math.Ceiling((double)_categoryService.GetAll().Count() / limit.Value);

            return View(listCategories);
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