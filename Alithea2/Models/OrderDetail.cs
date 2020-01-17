using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [DisplayName("Đơn giá")]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }

        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Màu")]
        public ColorProduct? Color { get; set; }
        public enum ColorProduct
        {
            [Display(Name = "Mặc định")]
            Default = 0,
            [Display(Name = "Đỏ")]
            color_Red = 2,
            [Display(Name = "Xanh")]
            color_Blue = 3,
            [Display(Name = "Vàng")]
            color_Yellow = 4,
            [Display(Name = "Xanh lá cây")]
            color_Green = 5,
        }

        public SizeProduct? Size { get; set; }
        public enum SizeProduct
        {
            M = 1,
            L = 0,
            XL = 2,
        }

        public void Display()
        {
            Debug.WriteLine("Id: " + ID);
            Debug.WriteLine("Order Id: " + OrderID);
            Debug.WriteLine("Product Id: " + ProductID);
            Debug.WriteLine("quantity: " + Quantity);
            Debug.WriteLine("price: " + UnitPrice);
            Debug.WriteLine("color: " + Color);
            Debug.WriteLine("size: " + Size);
        }
    }
}