using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Models;

namespace Alithea2.Controllers.Respository.ProductManager
{
    public class ProductRepository : BaseRepository<Product>
    {
        public List<Product> listPagination(int page, int limit)
        {

            List<Product> list = new List<Product>();
            try
            {
                list = _table.OrderByDescending(o => o.UpdatedAt).Skip(page).Take(limit).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return list;
        }
    }
}