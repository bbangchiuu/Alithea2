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

        [ForeignKey("Color")]
        public int? ColorID { get; set; }
        public virtual Color Color { get; set; }

        [DisplayName("Màu")]
        public string NameColor { get; set; }

        [ForeignKey("Size")]
        public int? SizeID { get; set; }
        public virtual Size Size { get; set; }

        [DisplayName("Size")]
        public string NameSize { get; set; }

        [DisplayName("Ảnh")]
        public string ProductImage { get; set; }

        public void Display()
        {
            Debug.WriteLine("Id: " + ID);
            Debug.WriteLine("Order Id: " + OrderID);
            Debug.WriteLine("Product Id: " + ProductID);
            Debug.WriteLine("quantity: " + Quantity);
            Debug.WriteLine("price: " + UnitPrice);
        }
    }
}