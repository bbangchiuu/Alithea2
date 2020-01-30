using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Alithea2.Controllers.Service.OrderManager;
using Alithea2.Models;
using Microsoft.Ajax.Utilities;

namespace Alithea2.Controllers
{
    public class OrdersController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private OrderService _orderService = new OrderService();
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

        // GET: Orders
        public ActionResult Index(int? page, int? limit, string start, string end)
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

            var orders = new List<Order>();
          
            ViewBag.CurrentPage = page;
            ViewBag.limit = limit;

            if (start.IsNullOrWhiteSpace() || end.IsNullOrWhiteSpace())
            {
                Debug.WriteLine("khong co date");
                try
                {
                    orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
                    ViewBag.TotalPage = Math.Ceiling((double)orders.Count() / limit.Value);
                    orders = orders.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("loi: " + e.Message);
                }
            }
            else
            {
                var startTime = DateTime.Now;
                startTime = startTime.AddYears(-1);
                try
                {
                    startTime = DateTime.Parse(start);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                var endTime = DateTime.Now;
                try
                {
                    endTime = DateTime.Parse(end);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    orders = db.Orders.OrderByDescending(s => s.OrderDate).Where(s => s.OrderDate >= startTime && s.OrderDate <= endTime).ToList();

                    ViewBag.TotalPage = Math.Ceiling((double)orders.Count() / limit.Value);

                    orders = orders.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

                    ViewBag.Start = startTime.ToString("yyyy-MM-dd");
                    ViewBag.End = endTime.ToString("yyyy-MM-dd");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return View(orders);
        }

        // GET: Orders/Details/5
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
            Order order = new Order();
            try
            {
                order = _orderService.SelectById(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                ViewBag.listOrderDetails = db.OrderDetails.Where(p => p.OrderID == id).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            order.Display();

            return View(order);
        }

        // GET: Orders/Edit/5
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
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserAccounts, "UserID", "RoleNumber", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,RoleNumber,OrderDate,RequireDate,ShippedDate,Quantity,TotalPrice,Commnet,UserID,FullName,Address,Phone,Email,Status")] Order order)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            Dictionary<string, string> errors = order.Validate();

            if (errors.Count == 0)
            {
                if (order.Status == Order.StatusOrder.Finish)
                {
                    order.ShippedDate = DateTime.Now;
                }

                if (ModelState.IsValid)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Errors = errors;
            ViewBag.UserID = new SelectList(db.UserAccounts, "UserID", "RoleNumber", order.UserID);
            return View(order);
        }

        private Order UpdateOrder(Order order)
        {
            List<OrderDetail> listOrderDetails = new List<OrderDetail>();
            int totalQuantity = 0;
            double totalPrice = 0;
            try
            {
                listOrderDetails = db.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();

                for (int i = 0; i < listOrderDetails.Count; i++)
                {
                    totalQuantity += listOrderDetails[i].Quantity;
                    totalPrice += listOrderDetails[i].UnitPrice * listOrderDetails[i].Quantity;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            order.Quantity = totalQuantity;
            order.TotalPrice = totalPrice;

            return order;
        }

        // GET: Orders/Delete/5
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
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            Order order = db.Orders.Find(id);
            order.Status = Order.StatusOrder.Deleted;
            db.Entry(order).State = EntityState.Modified;
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
