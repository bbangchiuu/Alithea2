using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class BuyItem
    {
        public List<Product> ListProducts = new List<Product>();

        public void AddProduct(Product product, int Quantity, int color, int size)
        {
            product.Quantity = Quantity;
            if (color != null)
            {
                product.Color = color;
            }

            if (size != null)
            {
                product.Size = size;
            }
            ListProducts.Add(product);
        }
    }
}