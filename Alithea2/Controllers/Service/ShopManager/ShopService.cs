using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.CategoryManager;
using Alithea2.Controllers.Respository.OrderManager;
using Alithea2.Controllers.Respository.ProductManager;
using Alithea2.Models;
using LinqKit;

namespace Alithea2.Controllers.Service.ShopManager
{
    public class ShopService : BaseService<BuyItem>
    {
        private OrderRespository _orderRespository = new OrderRespository();
        private CategoryRepository _categoryRepository = new CategoryRepository();
        private ProductRepository _productRepository = new ProductRepository();
        private MyDbContext _db = new MyDbContext();

        public Hashtable FilterProduct(List<int> id, int page, int limit, double? MinPrice, double? MaxPrice)
        {
            var hashtable = new Hashtable();
            var categoryFilter = new List<Category>();
            var productFilter = new List<Product>();
            var currentPara = "?";

            var predicate = PredicateBuilder.New<ProductCategory>();
            if (id != null)
            {
                for (int i = 0; i < id.Count; i++)
                {
                    int CatId = id[i];
                    predicate = predicate.Or(pc => pc.CategoryID == CatId);
                    categoryFilter.Add(_db.Categories.Find(CatId));

                    currentPara += "id=" + CatId + "&";
                }
            }

            //get category filter
            hashtable.Add("listCategory", categoryFilter);

            //get total product filter
            productFilter = _db.ProductCategories.Where(predicate.Compile()).Select(pc => pc)
                .GroupBy(pc => pc.ProductID).Where(pc => pc.Count() >= id.Count).Select(pc => pc.FirstOrDefault()?.Product).ToList();

            if (MinPrice != null || MaxPrice != null)
            {
                //filter product by price
                if (productFilter.Count == 0)
                {
                    productFilter = _db.Products.ToList();
                }

                if (MinPrice != null && MaxPrice == null)
                {
                    productFilter = productFilter.Where(p => p.UnitPrice >= MinPrice).ToList();
                }
                else if (MinPrice == null && MaxPrice != null)
                {
                    productFilter = productFilter.Where(p => p.UnitPrice <= MaxPrice).ToList();
                }
                else if (MinPrice != null && MaxPrice != null)
                {
                    productFilter = productFilter.Where(p => p.UnitPrice >= MinPrice && p.UnitPrice <= MaxPrice).ToList();
                }

                currentPara += "MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&";
            }

            //get listID
            hashtable.Add("currentPara", currentPara);
            //get total page filter
            hashtable.Add("totalPage", Math.Ceiling((double)productFilter.Count() / limit));

            //get product filter
            hashtable.Add("listProduct", productFilter.OrderByDescending(p => p.UpdatedAt).Skip((page - 1) * limit).Take(limit).ToList());

            return hashtable;
        }

        public Hashtable AddItem(List<Product> listShoppingCart, int proId, int quantity, int color, string nameColor, int size, string nameSize)
        {
            var hashtable = new Hashtable();
            if (listShoppingCart == null)
            {
                listShoppingCart = new List<Product>();
            }

            bool checkSp = true;
            for (var i = 0; i < listShoppingCart.Count; i++)
            {
                if (listShoppingCart[i].ProductID == proId && listShoppingCart[i].Color == color &&
                   listShoppingCart[i].Size == size)
                {
                   listShoppingCart[i].Quantity += quantity;
                   checkSp = false;
                }
            }

            if (checkSp)
            {
                var attr = _db.Attributes.FirstOrDefault(a => a.ProductID == proId && a.ColorID == color);
                var product = _productRepository.SelectById(proId);
                if (attr != null)
                {
                    product.ProductImage = attr.ProductImage;
                    product.UnitPrice = attr.UnitPrice;
                }
               
                product.Quantity = quantity;
                product.Color = color;
                product.NameColor = nameColor;
                product.Size = size;
                product.NameSize = nameSize;

                listShoppingCart.Add(product);
            }

            hashtable.Add("listShoppingCart", listShoppingCart);

            var getTotal = this.getTotal(listShoppingCart);
            hashtable.Add("TotalQuantity", getTotal["TotalQuantity"]);
            hashtable.Add("TotalPrice", getTotal["TotalPrice"]);

            return hashtable;
        }

        public Hashtable DeleteItem(List<Product> listShoppingCart, int idItem)
        {
            var hashtable = new Hashtable();
            try
            {
                listShoppingCart?.RemoveAt(idItem);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            
            hashtable.Add("listShoppingCart", listShoppingCart);

            var getTotal = this.getTotal(listShoppingCart);
            hashtable.Add("TotalQuantity", getTotal["TotalQuantity"]);
            hashtable.Add("TotalPrice", getTotal["TotalPrice"]);

            return hashtable;
        }

        public Hashtable UpdateItem(List<Product> listShoppingCart, int idItem, int quantity)
        {
            var hashtable = new Hashtable();
            if (listShoppingCart != null)
            {
                try
                {
                    if (quantity < 1)
                    {
                        listShoppingCart.RemoveAt(idItem);
                    }
                    else
                    {
                        listShoppingCart[idItem].Quantity = quantity;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            hashtable.Add("listShoppingCart", listShoppingCart);

            var getTotal = this.getTotal(listShoppingCart);
            hashtable.Add("TotalQuantity", getTotal["TotalQuantity"]);
            hashtable.Add("TotalPrice", getTotal["TotalPrice"]);

            return hashtable;
        }

        public Hashtable getTotal(List<Product> listShoppingCart)
        {
            var hashtable = new Hashtable();

            double totalPrice = 0;
            var totalQuantity = 0;

            if (listShoppingCart != null)
            {
                for (int i = 0; i < listShoppingCart.Count; i++)
                {
                    totalQuantity += listShoppingCart[i].Quantity;
                    totalPrice += listShoppingCart[i].UnitPrice * listShoppingCart[i].Quantity;
                }

                Debug.WriteLine("van co item");
            }

            hashtable.Add("TotalQuantity", totalQuantity);
            hashtable.Add("TotalPrice", totalPrice);

            return hashtable;
        }

        public bool createOrder(List<Product> listShoppingCart, DateTime createAt, Customer customer, int totalQuantity, double totalPrice, string comment, int? userID)
        {
            try
            {
                var order = new Order()
                {
                    RoleNumber = createAt.ToFileTimeUtc().ToString(),
                    OrderDate = createAt,
                    RequireDate = null,
                    ShippedDate = null,
                    Quantity = totalQuantity,
                    TotalPrice = totalPrice,
                    Status = Order.StatusOrder.DeActive,
                    UserID = userID,
                    Commnet = comment,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Address = customer.Address,
                    Phone = customer.Phone
                };

                return _orderRespository.createOrder(listShoppingCart, order);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}