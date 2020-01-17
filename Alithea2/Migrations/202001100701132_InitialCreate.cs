namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        RoleNumber = c.String(nullable: false, maxLength: 50),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                        CategoryImage = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID)
                .Index(t => t.RoleNumber, unique: true, name: "Ix_RoleNumber");
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        RoleNumber = c.String(nullable: false, maxLength: 50),
                        ProductName = c.String(),
                        ProductDescription = c.String(),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ProductImage = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .Index(t => t.RoleNumber, unique: true, name: "Ix_RoleNumber");
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        RoleNumber = c.String(nullable: false, maxLength: 50),
                        Username = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        Image = c.String(),
                        Password = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        CreatAt = c.DateTime(nullable: false),
                        DeleteAt = c.DateTime(),
                        UpdateAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                        admin = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .Index(t => t.RoleNumber, unique: true, name: "Ix_RoleNumber");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategories", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Customers", "Ix_RoleNumber");
            DropIndex("dbo.Products", "Ix_RoleNumber");
            DropIndex("dbo.ProductCategories", new[] { "CategoryID" });
            DropIndex("dbo.ProductCategories", new[] { "ProductID" });
            DropIndex("dbo.Categories", "Ix_RoleNumber");
            DropTable("dbo.Customers");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Categories");
        }
    }
}
