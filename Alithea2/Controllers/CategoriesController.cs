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
using Alithea2.Models;

namespace Alithea2.Controllers
{
    public class CategoriesController : Controller
    {
        private MyDbContext db = new MyDbContext();

        private CategoryService _categoryService = new CategoryService();
        private ProductCategoryService _productCategoryService = new ProductCategoryService();

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

        // GET: Categories
        public ActionResult Index(int? page, int? limit)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 6;
            }

            var listCategories = _categoryService.listPagination(page, limit);

            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;
            ViewBag.TotalPage = Math.Ceiling((double)_categoryService.GetAll().Count() / limit.Value);

            return View(listCategories);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id, int? page, int? limit)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 6;
            }

            //get category
            Category category = _categoryService.SelectById(id);
            
            if (category == null)
            {
                return HttpNotFound();
            }

            //get list product of category
            ViewBag.listProductCategories = _productCategoryService.GetProductOfCategoriesPagination(id, page, limit);

            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;
            ViewBag.TotalPage = Math.Ceiling((double)_productCategoryService.GetProductOfCategories(id).Count() / limit.Value);

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,RoleNumber,CategoryName,CategoryDescription,CategoryImage,CreatedAt,UpdatedAt,DeletedAt,Status")] Category category)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            //check error
            Dictionary<string, string> errors = category.Validate();
            if (errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View();
            }

            //add new category
            int newId = _categoryService.AddCategory(category);
            if (newId != 0)
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Đã xảy ra lỗi";
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = _categoryService.SelectById(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,RoleNumber,CategoryName,CategoryDescription,CategoryImage,CreatedAt,UpdatedAt,DeletedAt,Status")] Category category)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            Dictionary<string, string> errors = category.Validate();
            if (errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View();
            }

            category.Display();
            bool updateCat = _categoryService.UpdateCateogry(category);

            if (updateCat)
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Đã xảy ra lỗi";
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
//            Debug.WriteLine("dang xoa sss");
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

            Category category = new Category();
//            try
//            {
//                category = db.Categories.Find(id);
//                if (category == null)
//                {
//                    return HttpNotFound();
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (_categoryService.DeleteCategory(id))
            {
                return RedirectToAction("Index");
            }
            //            try
            //            {
            //                category = db.Categories.Find(id);
            //                category.Status = Category.CategoryStatus.Deleted;
            //                db.Entry(category).State = EntityState.Modified;
            //                db.SaveChanges();
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e);
            //            }

            TempData["Error"] = "Đã xảy ra lỗi";
            return RedirectToAction("Details/" + id);
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
