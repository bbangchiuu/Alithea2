using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Alithea2.Models;

namespace Alithea2.Controllers
{
    public class UserAccountsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: UserAccounts
        public ActionResult Index()
        {
            return View(db.UserAccounts.ToList());
        }

        // GET: UserAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }
//
//        // GET: UserAccounts/Create
//        public ActionResult Create()
//        {
//            return View();
//        }
//
//        // POST: UserAccounts/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "UserID,RoleNumber,Username,FullName,Phone,Address,Email,Image,Password,BirthDay,CreatAt,DeleteAt,UpdateAt,Status,admin")] UserAccount userAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                db.UserAccounts.Add(userAccount);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//
//            return View(userAccount);
//        }

        // GET: UserAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,RoleNumber,Username,FullName,Phone,Address,Email,Image,Password,BirthDay,CreatAt,DeleteAt,UpdateAt,Status,admin")] UserAccount userAccount)
        {
            userAccount.UpdateAt = DateTime.Now;
            if (userAccount.Status == UserAccount.UserAccountStatus.Deleted)
            {
                userAccount.DeleteAt = DateTime.Now;
            }
            else
            {
                userAccount.DeleteAt = null;
            }
            userAccount.Display();

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(userAccount).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("loi: " + e.Message);
                TempData["Error"] = "Đã xảy ra lỗi";
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            UserAccount userAccount = db.UserAccounts.Find(id);
//            if (userAccount == null)
//            {
//                return HttpNotFound();
//            }
//            return View(userAccount);
//        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount.admin == UserAccount.Decentralization.Admin)
            {
                TempData["Error"] = "Đây là tài khoản Admin, không xóa được";
                return RedirectToAction("Edit/" + userAccount.UserID);
            }

            db.UserAccounts.Remove(userAccount);
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
