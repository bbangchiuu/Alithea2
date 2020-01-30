using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Models;
using Attribute = Alithea2.Models.Attribute;

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

        public bool AddProductAndListCategoryOfProduct(Product product, List<int> catId, List<int> idColor, List<int> quanityColor, List<double> priceColor, List<string> imageColor)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Products.Add(product);
                    _db.SaveChanges();

                    for (int i = 0; i < catId.Count; i++)
                    {
                        _db.ProductCategories.Add(new ProductCategory()
                        {
                            ProductID = product.ProductID,
                            CategoryID = catId[i],
                        });
                    }
                    _db.SaveChanges();

                    //add new attribute
                    if (idColor != null && idColor.Count > 0)
                    {
                        for (int i = 0; i < idColor.Count; i++)
                        {
                            if (imageColor[i].Equals("default"))
                            {
                                imageColor[i] = product.ProductImage;
                            }

                            if (priceColor[i] <= 0)
                            {
                                priceColor[i] = product.UnitPrice;
                            }
                            _db.Attributes.Add(new Attribute()
                            {
                                ProductID = product.ProductID,
                                ColorID = idColor[i],
                                ProductImage = imageColor[i],
                                Quantity = quanityColor[i],
                                UnitPrice = priceColor[i]
                            });
                        }
                        _db.SaveChanges();
                    }

                    transaction.Commit();
                    return true;

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Debug.WriteLine(e);
                    return false;
                }
            }
        }

        public bool UpdateProduct(Product product, List<int> catId, List<int> idColor, List<int> quanityColor, List<double> priceColor, List<string> imageColor)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //update product
                    _db.Products.Attach(product);
                    _db.Entry(product).State = EntityState.Modified;

                    //remove old category of product 
                    var listProductCateogry = _db.ProductCategories.Where(pc => pc.ProductID == product.ProductID).ToList();
                    _db.ProductCategories.RemoveRange(listProductCateogry);

                    //add new category of product 
                    for (int i = 0; i < catId.Count; i++)
                    {
                        _db.ProductCategories.Add(new ProductCategory()
                        {
                            ProductID = product.ProductID,
                            CategoryID = catId[i],
                        });
                    }

                    //remove old attribute
                    var listPro = _db.Attributes.Where(a => a.ProductID == product.ProductID).ToList();
                    _db.Attributes.RemoveRange(listPro);

                    //add new attribute
                    if (idColor != null && idColor.Count > 0)
                    {
                        if (idColor != null && idColor.Count > 0)
                        {
                            for (int i = 0; i < idColor.Count; i++)
                            {
                                if (imageColor[i].Equals("default"))
                                {
                                    imageColor[i] = product.ProductImage;
                                }

                                if (priceColor[i] <= 0)
                                {
                                    priceColor[i] = product.UnitPrice;
                                }
                                _db.Attributes.Add(new Attribute()
                                {
                                    ProductID = product.ProductID,
                                    ColorID = idColor[i],
                                    ProductImage = imageColor[i],
                                    Quantity = quanityColor[i],
                                    UnitPrice = priceColor[i]
                                });
                            }
                        }
                    }
                    _db.SaveChanges();

                    transaction.Commit();
                    return true;

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Debug.WriteLine(e);
                    return false;
                }
            }
        }
    }
}