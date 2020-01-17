namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAttibuteProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Color", c => c.Int());
            AddColumn("dbo.OrderDetails", "Size", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Size");
            DropColumn("dbo.OrderDetails", "Color");
        }
    }
}
