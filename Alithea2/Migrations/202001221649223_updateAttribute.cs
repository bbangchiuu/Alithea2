namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAttribute : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attributes", "SizeID", "dbo.Sizes");
            DropIndex("dbo.Attributes", new[] { "SizeID" });
            DropColumn("dbo.Attributes", "SizeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attributes", "SizeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Attributes", "SizeID");
            AddForeignKey("dbo.Attributes", "SizeID", "dbo.Sizes", "SizeID", cascadeDelete: true);
        }
    }
}
