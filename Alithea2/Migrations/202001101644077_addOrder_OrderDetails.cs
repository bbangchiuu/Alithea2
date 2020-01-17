namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrder_OrderDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        RoleNumber = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        RequireDate = c.DateTime(),
                        ShippedDate = c.DateTime(),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Commnet = c.String(),
                        UserID = c.Int(),
                        FullName = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.UserAccounts", t => t.UserID)
                .Index(t => t.RoleNumber, unique: true, name: "Ix_RoleNumber")
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserID", "dbo.UserAccounts");
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.Orders", "Ix_RoleNumber");
            DropIndex("dbo.OrderDetails", new[] { "ProductID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
