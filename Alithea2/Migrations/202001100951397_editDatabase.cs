namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDatabase : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customers", "Ix_RoleNumber");
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.UserID)
                .Index(t => t.RoleNumber, unique: true, name: "Ix_RoleNumber");
            
            DropTable("dbo.Customers");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.CustomerID);
            
            DropIndex("dbo.UserAccounts", "Ix_RoleNumber");
            DropTable("dbo.UserAccounts");
            CreateIndex("dbo.Customers", "RoleNumber", unique: true, name: "Ix_RoleNumber");
        }
    }
}
