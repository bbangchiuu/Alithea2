using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Alithea2.Models
{
    public class ProductCategory
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public void Display()
        {
            Debug.WriteLine("id: " + ID);
            Debug.WriteLine("Proid: " + ProductID);
            Debug.WriteLine("Catid: " + CategoryID);
        }
    }
}