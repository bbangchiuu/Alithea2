namespace Alithea2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=MyDbContext")
        {
        }


        public System.Data.Entity.DbSet<Alithea2.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.ProductCategory> ProductCategories { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.UserAccount> UserAccounts { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.Color> Colors { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.Size> Sizes { get; set; }

        public System.Data.Entity.DbSet<Alithea2.Models.Attribute> Attributes { get; set; }
    }
}
