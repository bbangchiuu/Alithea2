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
    public class Attribute
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [DisplayName("Ảnh")]
        public string ProductImage { get; set; }

        [ForeignKey("Color")]
        public int ColorID { get; set; }
        public virtual Color Color { get; set; }

        [DisplayName("Đơn giá")]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }

        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        public void Display()
        {
            Debug.WriteLine("id: " + ID);
            Debug.WriteLine("quanity: " + Quantity);
            Debug.WriteLine("price: " + UnitPrice);
            Debug.WriteLine("image: " + ProductImage);
            Debug.WriteLine("product id: " + ProductID);
            Debug.WriteLine("color id: " + ColorID);
        }
    }
}