using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.CategoryManager;
using Alithea2.Models;

namespace Alithea2.Controllers.Service.CategoryManager
{
    public class CategoryService : BaseService<Category>
    {
        CategoryRepository _categoryRepository = new CategoryRepository();
     
        public List<Category> listPagination(int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 3;
            }

            return _categoryRepository.listPagination((page.Value - 1) * limit.Value, limit.Value);
        }

        public int AddCategory(Category category)
        {
            _categoryRepository.Insert(category);
            try
            {
                _categoryRepository.Save();
                return category.CategoryID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool UpdateCateogry(Category category)
        {
            try
            {
                category.UpdatedAt = DateTime.Now;
                if (category.Status == Category.CategoryStatus.Deleted)
                {
                    category.DeletedAt = DateTime.Now;
                }

                _baseRepo.Update(category);
                _baseRepo.Save();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                Category category = _categoryRepository.SelectById(id);
                if (category == null)
                {
                    return false;
                }

                category.Status = Category.CategoryStatus.Deleted;
                category.DeletedAt = DateTime.Now;

                _categoryRepository.Update(category);
                _categoryRepository.Save();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("loi ne: " + e.Message);
                return false;
            }
            
        }
    }
}