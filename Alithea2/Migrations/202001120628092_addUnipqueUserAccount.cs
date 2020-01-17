namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUnipqueUserAccount : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserAccounts", "Ix_RoleNumber");
            AlterColumn("dbo.UserAccounts", "RoleNumber", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.UserAccounts", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.UserAccounts", "Email", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.UserAccounts", "RoleNumber", unique: true, name: "Ix_RoleNumber");
            CreateIndex("dbo.UserAccounts", "Username", unique: true, name: "Ix_Username");
            CreateIndex("dbo.UserAccounts", "Email", unique: true, name: "Ix_Email");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserAccounts", "Ix_Email");
            DropIndex("dbo.UserAccounts", "Ix_Username");
            DropIndex("dbo.UserAccounts", "Ix_RoleNumber");
            AlterColumn("dbo.UserAccounts", "Email", c => c.String());
            AlterColumn("dbo.UserAccounts", "Username", c => c.String());
            AlterColumn("dbo.UserAccounts", "RoleNumber", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.UserAccounts", "RoleNumber", unique: true, name: "Ix_RoleNumber");
        }
    }
}
