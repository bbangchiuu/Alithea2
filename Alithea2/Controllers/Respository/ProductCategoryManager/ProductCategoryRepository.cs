using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Models;

namespace Alithea2.Controllers.Respository.ProductCategoryManager
{
    public class ProductCategoryRepository : BaseRepository<ProductCategory>
    {
        public List<ProductCategory> GetProductOfCategories(int Catid)
        {
            List<ProductCategory> listProductCategories = new List<ProductCategory>();

            try
            {
                listProductCategories = _db.ProductCategories.Where(p => p.CategoryID == Catid).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return listProductCategories;
        }

        public List<ProductCategory> GetProductOfCategoriesPagination(int Catid, int page, int limit)
        {
            List<ProductCategory> listProductCategories = new List<ProductCategory>();
            try
            {
                listProductCategories = _db.ProductCategories.OrderByDescending(p => p.ProductID).Where(p => p.CategoryID == Catid).Skip(page).Take(limit).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return listProductCategories;
        }

        public List<ProductCategory> GetCategories(int id)
        {
            List<ProductCategory> listProductCategories = new List<ProductCategory>();

            try
            {
                listProductCategories = _table.Where(pc => pc.ProductID == id).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return listProductCategories;
        }
    }
}