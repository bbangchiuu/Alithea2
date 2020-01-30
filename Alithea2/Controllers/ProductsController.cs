using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Alithea2.Controllers.Service.CategoryManager;
using Alithea2.Controllers.Service.ProductCateogryManager;
using Alithea2.Controllers.Service.ProductManager;
using Alithea2.Models;

namespace Alithea2.Controllers
{
    public class ProductsController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private ProductService _productService = new ProductService();
        private ProductCategoryService _productCategoryService = new ProductCategoryService();
        private CategoryService _categoryService = new CategoryService();

        public bool CheckUser()
        {

            UserAccount userAccount = new UserAccount();
            if (Session["UserAccount"] != null)
            {
                userAccount = Session["UserAccount"] as UserAccount;
            }
            else
            {
                return false;
            }

            Debug.WriteLine("admin: " + userAccount.admin);

            if (userAccount.admin == null || userAccount.admin == 0)
            {
                Debug.WriteLine("Dang chay");
                return false;
            }

            return true;
        }

        // GET: Products
        public ActionResult Index(int? page, int? limit)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }

            var listProducts = _productService.listPagination(page, limit);

            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;
            ViewBag.TotalPage = Math.Ceiling((double)_productService.GetAll().Count() / limit.Value);

            return View(listProducts);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _productService.SelectById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.listProductCategories = _productCategoryService.GetCategories(id);

            //list attribute
            ViewBag.listPro = db.Attributes.Where(a => a.ProductID == id).ToList();

            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }
            ViewBag.Colos = db.Colors.ToList();
            ViewBag.Sizes = db.Sizes.ToList();
            ViewBag.listCategories = _categoryService.GetAll().ToList();
           
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<int> ints, List<int> idColor, List<int> quanityColor, List<double> priceColor, List<string> imageColor)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            //CHECK ERROR
            Dictionary<string, string> errors = product.Validate();
        
            if (errors.Count == 0 && ints != null && ints.Count > 0)
            {
                if (_productService.AddProductAndListCategoryOfProduct(product, ints, idColor, quanityColor, priceColor, imageColor))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Đã xảy ra lỗi";
                }
            }
            else
            {
                errors.Add("Category", "Bạn chưa chọn danh mục cho sản phẩm");
            }

            ViewBag.Errors = errors;

            //GET LIST CATEGORY
            ViewBag.listCategories = _categoryService.GetAll().ToList();

            //list color
            ViewBag.Colos = db.Colors.ToList();

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            //check id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _productService.SelectById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            //get list category
            ViewBag.listCategories = _categoryService.GetAll().ToList();

            //get category of product
            ViewBag.listProductCategories = _productCategoryService.GetCategories(id);

            //list color
            ViewBag.Colos = db.Colors.ToList();

            //list attribute
            ViewBag.listPro = db.Attributes.Where(a => a.ProductID == id).ToList();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, List<int> ints, List<int> idColor, List<int> quanityColor, List<double> priceColor, List<string> imageColor)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            product.Display();

            //CHECK ERROR
            Dictionary<string, string> errors = product.Validate();

            if (errors.Count == 0 && ints != null && ints.Count > 0)
            {
                if (_productService.UpdateProduct(product, ints, idColor, quanityColor, priceColor, imageColor))
                {
                    return RedirectToAction("Details/" + product.ProductID);
                }
                else
                {
                    TempData["Error"] = "Đã xảy ra lỗi";
                }
            }
            else
            {
                errors.Add("Category", "Bạn chưa chọn danh mục cho sản phẩm");
            }

            ViewBag.Errors = errors;

            //GET LIST CATEGORY
            ViewBag.listCategories = _categoryService.GetAll().ToList();

            //get category of product
            ViewBag.listProductCategories = _productCategoryService.GetCategories(product.ProductID);

            return View(product);
        }

        // GET: Products/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }
//
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Product product = db.Products.Find(id);
//            if (product == null)
//            {
//                return HttpNotFound();
//            }
//            return View(product);
//        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (_productService.DeleteProduct(id))
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Đã xảy ra lỗi";
            return RedirectToAction("Edit/" + id);
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
