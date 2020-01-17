using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.ProductCategoryManager;
using Alithea2.Models;

namespace Alithea2.Controllers.Service.ProductCateogryManager
{
    public class ProductCategoryService : BaseService<ProductCategory>
    {
        ProductCategoryRepository _productCategoryRepository = new ProductCategoryRepository();

        public List<ProductCategory> GetProductOfCategories(int? Catid)
        {
            if (Catid == null)
            {
                Catid = 0;
            }
            return _productCategoryRepository.GetProductOfCategories(Catid.Value);
        }


        public List<ProductCategory> GetProductOfCategoriesPagination(int? Catid, int? page, int? limit)
        {
            if (Catid == null)
            {
                Catid = 0;
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }

            return _productCategoryRepository.GetProductOfCategoriesPagination(Catid.Value, (page.Value - 1) * limit.Value, limit.Value);
        }

        public List<ProductCategory> GetCategories(int? id)
        {
            if (id == null)
            {
                id = 0;
            }

            return _productCategoryRepository.GetCategories(id.Value);
        }
    }
}