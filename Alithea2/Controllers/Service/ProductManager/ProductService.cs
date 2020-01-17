using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.ProductManager;
using Alithea2.Models;

namespace Alithea2.Controllers.Service.ProductManager
{
    public class ProductService : BaseService<Product>
    {
        private ProductRepository _productRepository = new ProductRepository();

        public List<Product> listPagination(int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }

            return _productRepository.listPagination((page.Value - 1) * limit.Value, limit.Value);
        }

        public int AddProduct(Product product)
        {
            if (product == null)
            {
                return 0;
            }

            _productRepository.Insert(product);
            try
            {
                _productRepository.Save();
                return product.ProductID;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}