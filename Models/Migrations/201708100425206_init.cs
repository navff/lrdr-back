namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Text = c.String(),
                        Time = c.DateTimeOffset(nullable: false, precision: 7),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerUserId = c.Int(nullable: false),
                        Name = c.String(),
                        Deadline = c.DateTimeOffset(nullable: false, precision: 7),
                        CustomerUserId = c.Int(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DeliveryAddress = c.String(),
                        ShowPayment = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerUserId)
                .ForeignKey("dbo.Users", t => t.OwnerUserId)
                .Index(t => t.OwnerUserId)
                .Index(t => t.CustomerUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255),
                        AuthToken = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(maxLength: 255, unicode: false),
                        Phone = c.String(maxLength: 255, unicode: false),
                        AvatarFileId = c.Int(),
                        Role = c.Int(nullable: false),
                        DateRegistered = c.DateTimeOffset(nullable: false, precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.AvatarFileId)
                .Index(t => t.Email, unique: true)
                .Index(t => t.AuthToken)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.AvatarFileId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Extension = c.String(),
                        LinkedObjectId = c.Int(),
                        LinkedObjectType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Time = c.DateTimeOffset(nullable: false, precision: 7),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExternalId = c.String(),
                        ExternalUserId = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Comments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OwnerUserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "CustomerUserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "AvatarFileId", "dbo.Files");
            DropIndex("dbo.Payments", new[] { "OrderId" });
            DropIndex("dbo.Users", new[] { "AvatarFileId" });
            DropIndex("dbo.Users", new[] { "Phone" });
            DropIndex("dbo.Users", new[] { "Name" });
            DropIndex("dbo.Users", new[] { "AuthToken" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Orders", new[] { "CustomerUserId" });
            DropIndex("dbo.Orders", new[] { "OwnerUserId" });
            DropIndex("dbo.Comments", new[] { "OrderId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Files");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Comments");
        }
    }
}
