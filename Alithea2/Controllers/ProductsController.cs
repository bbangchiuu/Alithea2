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

            ViewBag.listCat = _productCategoryService.GetCategories(id);
          
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            ViewBag.listCategories = _categoryService.GetAll().ToList();
           
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,RoleNumber,ProductName,ProductDescription,UnitPrice,Quantity,ProductImage,CreatedAt,UpdatedAt,DeletedAt,Status")] Product product, List<int> ints)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            //CHECK ERROR
            Dictionary<string, string> errors = product.Validate();
        
            if (errors.Count == 0 && ints != null && ints.Count > 0)
            {
                //SET UP PRODUCT
                product.Display();

                using (_productService.BeginTransaction())
                {
                    try
                    {
                        int pro_id = _productService.AddProduct(product);
                        product.Display();
                        if (pro_id > 0)
                        {
                            for (int i = 0; i < ints.Count; i++)
                            {
                                _productCategoryService.Insert(new ProductCategory
                                {
                                    ProductID = 0,
                                    CategoryID = ints[i]
                                });
                            }

                            _productCategoryService.Save();
                        }

                        _productService.CommitTransaction();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        _productService.RollBackTransaction();
                        Debug.WriteLine("loi: " + e.Message);
                    }
                }
            }
            else
            {
                errors.Add("Category", "Bạn chưa chọn danh mục cho sản phẩm");
            }

            TempData["Error"] = "Đã xảy ra lỗi";
            ViewBag.Errors = errors;
            //GET LIST CATEGORY
            ViewBag.listCategories = _categoryService.GetAll().ToList();

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

            //get list category
            List<Category> listCategories = new List<Category>();
            try
            {
                listCategories = db.Categories.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            ViewBag.listCategories = listCategories;

            //get product
            Product product = new Product();
            List<ProductCategory> listProductCategories = new List<ProductCategory>();

            try
            {
                product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                //get category of product
                listProductCategories = db.ProductCategories.Where(pc => pc.ProductID == id).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            ViewBag.listProductCategories = listProductCategories;

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,RoleNumber,ProductName,ProductDescription,UnitPrice,Quantity,ProductImage,CreatedAt,UpdatedAt,DeletedAt,Status")] Product product, List<int> ints)
        {
//            if (!CheckUser())
//            {
//                return Redirect("/Home/Login");
//            }

            product.Display();
            //GET LIST CATEGORY
            List<Category> listCategories = new List<Category>();
            try
            {
                listCategories = db.Categories.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            ViewBag.listCategories = listCategories;

            //get category of product
            List<ProductCategory> listProductCategories = new List<ProductCategory>();
            try
            {
                listProductCategories = db.ProductCategories.Where(pc => pc.ProductID == product.ProductID).ToList();
                
                Debug.WriteLine("dang chay");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            Debug.WriteLine("count1: " + listProductCategories.Count);

            //CHECK ERROR
            Dictionary<string, string> errors = product.Validate();
            if (ints != null)
            {
                for (int i = 0; i < ints.Count; i++)
                {
                    Debug.WriteLine("id: " + ints[i]);
                }
            }
            else
            {
                Debug.WriteLine("khong co");
                errors.Add("Category", "Bạn chưa chọn danh mục cho sản phẩm");
            }

            for (int index = 0; index < errors.Count; index++)
            {
                var item = errors.ElementAt(index);
                Debug.WriteLine(item.Key + " - " + item.Value);
//                var itemKey = item.Key;
//                var itemValue = item.Value;
            }

            if (errors.Count == 0 && ints != null)
            {
                product.UpdatedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    //update product
                    db.Entry(product).State = EntityState.Modified;

                    //remove category of product
                    db.ProductCategories.RemoveRange(listProductCategories);

                    //add category of product
                    listProductCategories = new List<ProductCategory>();
                    for (int i = 0; i < ints.Count; i++)
                    {
                        ProductCategory newProductCategory = new ProductCategory
                        {
                            ProductID = product.ProductID,
                            CategoryID = ints[i]
                        };
                        listProductCategories.Add(newProductCategory);
                    }
                    db.ProductCategories.AddRange(listProductCategories);

                    //save database
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Debug.WriteLine("khong hop le");
                }
            }
            else
            {
                Debug.WriteLine("dang loi");
            }

            ViewBag.Errors = errors;
            Debug.WriteLine("count2: " + listProductCategories.Count);
            for (int i = 0; i < listProductCategories.Count; i++)
            {
                listProductCategories[i].Display();
            }
            ViewBag.listProductCategories = listProductCategories;

            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            Product product = db.Products.Find(id);
            product.Status = Product.ProductStatus.Deleted;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
