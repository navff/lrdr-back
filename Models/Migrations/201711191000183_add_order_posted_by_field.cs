namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_order_posted_by_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PostedByUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "PostedByUserId");
            AddForeignKey("dbo.Orders", "PostedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "PostedByUserId", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "PostedByUserId" });
            DropColumn("dbo.Orders", "PostedByUserId");
        }
    }
}
