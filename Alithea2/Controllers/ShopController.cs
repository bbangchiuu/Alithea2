using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alithea2.Models;

namespace Alithea2.Controllers
{
    public class ShopController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Shop
        public ActionResult QuanAoNam()
        {
            var listProNam = db.ProductCategories.Where(pc => pc.CategoryID == 1).Take(4).ToList();
            return PartialView(listProNam);
        }

        public ActionResult QuanAoNu()
        {
            var listProNu = db.ProductCategories.Where(pc => pc.CategoryID == 2).Take(4).ToList();
            return PartialView(listProNu);
        }

        public ActionResult GiayNam()
        {
            var listGiayNam = db.ProductCategories.Where(pc => pc.CategoryID == 4).Take(4).ToList();
            return PartialView(listGiayNam);
        }

        public ActionResult GiayNu()
        {
            var listGiayNu = db.ProductCategories.Where(pc => pc.CategoryID == 5).Take(4).ToList();
            return PartialView(listGiayNu);
        }
    }
}