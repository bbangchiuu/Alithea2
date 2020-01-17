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
    public class OrderDetailsController : Controller
    {
        private MyDbContext db = new MyDbContext();


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

        // GET: OrderDetails
        public ActionResult Index()
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            var orderDetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (!CheckUser())
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "RoleNumber");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "RoleNumber");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,ProductID,UnitPrice,Quantity,Color,Size")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "RoleNumber", orderDetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "RoleNumber", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.listProducts = db.Products.ToList();
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,ProductID,UnitPrice,Quantity,Color,Size")] OrderDetail orderDetail)
        {
            orderDetail.Display();
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();

                Order order = db.Orders.Find(orderDetail.OrderID);
                if (order == null)
                {
                    return HttpNotFound();
                }

                order = UpdateOrder(order);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("/Orders/Details/" + order.OrderID);
            }
            ViewBag.listProducts = db.Products.ToList();
            return View(orderDetail);
        }

//        // GET: OrderDetails/Delete/5
//        public ActionResult Delete(int? id, int? orderID)
//        {
//            if (id == null || orderID == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//
//            OrderDetail orderDetail = db.OrderDetails.Find(id);
//            if (orderDetail == null)
//            {
//                return HttpNotFound();
//            }
//            Order order = db.Orders.Find(orderID);
//            if (order == null)
//            {
//                return HttpNotFound();
//            }
//
//            db.OrderDetails.Remove(orderDetail);
//            db.SaveChanges();
//
//            order = UpdateOrder(order);
//            db.Entry(order).State = EntityState.Modified;
//
//            return Redirect("/Orders/Details/" + orderID);
//        }

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

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();

            Order order = db.Orders.Find(orderDetail.OrderID);
            if (order == null)
            {
                return HttpNotFound();
            }

            order = UpdateOrder(order);
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("/Orders/Details/" + order.OrderID);
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
