namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_attribute : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        ProductImage = c.String(),
                        ColorID = c.Int(nullable: false),
                        SizeID = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Colors", t => t.ColorID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ColorID)
                .Index(t => t.SizeID);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ColorID = c.Int(nullable: false, identity: true),
                        NameColor = c.String(),
                    })
                .PrimaryKey(t => t.ColorID);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        SizeID = c.Int(nullable: false, identity: true),
                        NameSize = c.String(),
                    })
                .PrimaryKey(t => t.SizeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attributes", "SizeID", "dbo.Sizes");
            DropForeignKey("dbo.Attributes", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Attributes", "ColorID", "dbo.Colors");
            DropIndex("dbo.Attributes", new[] { "SizeID" });
            DropIndex("dbo.Attributes", new[] { "ColorID" });
            DropIndex("dbo.Attributes", new[] { "ProductID" });
            DropTable("dbo.Sizes");
            DropTable("dbo.Colors");
            DropTable("dbo.Attributes");
        }
    }
}
