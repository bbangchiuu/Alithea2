namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOderDetailAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ColorID", c => c.Int());
            AddColumn("dbo.OrderDetails", "NameColor", c => c.String());
            AddColumn("dbo.OrderDetails", "SizeID", c => c.Int());
            AddColumn("dbo.OrderDetails", "NameSize", c => c.String());
            AddColumn("dbo.OrderDetails", "ProductImage", c => c.String());
            CreateIndex("dbo.OrderDetails", "ColorID");
            CreateIndex("dbo.OrderDetails", "SizeID");
            AddForeignKey("dbo.OrderDetails", "ColorID", "dbo.Colors", "ColorID");
            AddForeignKey("dbo.OrderDetails", "SizeID", "dbo.Sizes", "SizeID");
            DropColumn("dbo.OrderDetails", "Color");
            DropColumn("dbo.OrderDetails", "Size");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Size", c => c.Int());
            AddColumn("dbo.OrderDetails", "Color", c => c.Int());
            DropForeignKey("dbo.OrderDetails", "SizeID", "dbo.Sizes");
            DropForeignKey("dbo.OrderDetails", "ColorID", "dbo.Colors");
            DropIndex("dbo.OrderDetails", new[] { "SizeID" });
            DropIndex("dbo.OrderDetails", new[] { "ColorID" });
            DropColumn("dbo.OrderDetails", "ProductImage");
            DropColumn("dbo.OrderDetails", "NameSize");
            DropColumn("dbo.OrderDetails", "SizeID");
            DropColumn("dbo.OrderDetails", "NameColor");
            DropColumn("dbo.OrderDetails", "ColorID");
        }
    }
}
