using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class Color
    {
        [Key]
        public int ColorID { get; set; }

        [DisplayName("Màu")]
        public string NameColor { get; set; }

        public ICollection<Attribute> Attributes { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}