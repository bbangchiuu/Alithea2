using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }

        [DisplayName("Size")]
        public string NameSize { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}